static void start(string[] args) {
    Configuration configuration = Configurator.Build();

    Optional<ServiceManagerEvents> _ = LoadServiceManagerEvents(configuration);
    
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

static Optional<ServiceManagerEvents> LoadServiceManagerEvents(Configuration configuration) {
    Optional<ServiceManagerEvents> manager = configuration.container.GetEvents();
    if(manager.IsSome()) {
        manager.Unwrap().Execute();
    } else {
        Console.WriteLine("Events couldn't be loaded.");
    }

    return manager;
}

static ServiceWeb LoadServiceWeb(Configuration configuration) {
    Optional<IRepository> oRepository = configuration.container.Repository;
    IRepository repository = new RepositorMemory(configuration.persistence);
    if(oRepository.IsSome()) {
        repository = oRepository.Unwrap();
    } else {
        Console.WriteLine("User repository couldn't be loaded, memory instance implemented.");
    }

    BuilderServiceWeb serviceBuilder = new(repository);
    foreach (var handler in configuration.webHandlers) {
        serviceBuilder.SetHandler(handler);
    }
    return serviceBuilder.Build();
}

start(args);