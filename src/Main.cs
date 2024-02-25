static void start(string[] args) {
    Configuration configuration = Configurator.Build();

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
    foreach (var handler in configuration.WebHandlers()) {
        serviceBuilder.SetHandler(handler);
    }
    return serviceBuilder.Build();
}

static ServiceManagerEvents LoadServiceEvent(Configuration configuration) {
    BuilderServiceManagerEvents serviceBuilder = new BuilderServiceManagerEvents();
    foreach (var service in configuration.ServiceEvents()) {
        serviceBuilder.SetService(service.Item1, service.Item2);
    }
    return serviceBuilder.Build();
}

start(args);