static void start(string[] args) {
    Configuration configuration = Configurator.Build();

    IRepository repository = new RepositoryMySQL(configuration.persistence);

    LogEvent log = new LogEvent("TestService", "001", "TEST", "SELF", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond, "This is a test message");
    repository.Insert(log);
    repository.FindAll(log.Service, log.SessionId);

    ServiceManagerEvents manager = LoadServiceEvent(configuration);
    manager.Execute();

    WebApplication app = LoadApp(args);

    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    ServiceWeb service = LoadServiceWeb(configuration);
    app = Controller.Initialize(app, service);

    app.Run();
}

static WebApplication LoadApp(string[] args) {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    return builder.Build();
}

static ServiceWeb LoadServiceWeb(Configuration configuration) {
    BuilderServiceWeb serviceBuilder = new BuilderServiceWeb();
    foreach (var handler in configuration.webHandlers) {
        serviceBuilder.SetHandler(handler);
    }
    return serviceBuilder.Build();
}

static ServiceManagerEvents LoadServiceEvent(Configuration configuration) {
    BuilderServiceManagerEvents serviceBuilder = new BuilderServiceManagerEvents();
    foreach (var service in configuration.serviceEvents) {
        serviceBuilder.SetService(service.Item1, service.Item2);
    }
    return serviceBuilder.Build();
}

start(args);