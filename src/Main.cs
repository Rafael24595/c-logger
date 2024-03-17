static void start(string[] args) {
    Configuration configuration = Configurator.Build();
    
    WebApplication app = LoadWeb(args);

    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app = LoadApp(app, configuration);

    app.Run();
}

static WebApplication LoadWeb(string[] args) {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    return builder.Build();
}

static WebApplication LoadApp(WebApplication app, Configuration configuration) {
    Optional<IRepository> oRepository = configuration.container.Repository;
    IRepository repository = new RepositorMemory(configuration.persistence);
    if(oRepository.IsSome()) {
        repository = oRepository.Unwrap();
    } else {
        Console.WriteLine("User repository couldn't be loaded, memory instance implemented.");
    }

    BuilderServiceWeb serviceBuilder = new(repository);
    foreach (var module in configuration.modules) {
        module.Initialize(app, serviceBuilder);
    }

    foreach (var module in configuration.closeableModules) {
        module.Initialize(app, serviceBuilder);
    }

    app = Controller.Initialize(app, serviceBuilder.Build());

    app.Urls.Add(configuration.host);

    return app;
}

start(args);