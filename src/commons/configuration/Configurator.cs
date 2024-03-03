public class Configurator {

    public static Configuration Build() {
        IConfigurationSection config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build()
            .GetSection("AppConfig");

        List<string> webHandlers = [];
        foreach (var item in config.GetSection("WebHandlers").GetChildren()) {
            if(item.Value != null) {
                webHandlers.Add(item.Value);
                Console.WriteLine(item.Value);
            }
        }

        Persistence persistence = LoadPersistence(config);
        Optional<IRepository> repository = RepositoryDictionary.Find(persistence);
        ServiceManagerEvents events = LoadServiceEvent(config);

        DependencyContainer container = LoadDependencyContainer(events, repository);

        return new(container, [.. webHandlers], persistence);
    }

     private static Persistence LoadPersistence(IConfigurationSection config) {
        var persistence = config.GetSection("Persistence");
        
        var pCode = persistence.GetValue<string>("code") ?? "";
        var pConnection = persistence.GetValue<string>("connection") ?? "";
        var pDatabase = persistence.GetValue<string>("database") ?? "";
        var pCollection = persistence.GetValue<string>("collection") ?? "";

        return new(pCode, pConnection, pDatabase, pCollection);
    }

    private static DependencyContainer LoadDependencyContainer(ServiceManagerEvents events, Optional<IRepository> repository) {
        DependencyContainer container = new DependencyContainer()
            .SetEvents(events)
            .SetRepository(repository);

        return container;
    }

    private static ServiceManagerEvents LoadServiceEvent(IConfigurationSection config) {
        BuilderServiceManagerEvents serviceBuilder = new BuilderServiceManagerEvents();

        foreach (var item in config.GetSection("ServiceEvents").GetChildren()) {
            var code = item.GetValue<string>("code");
            var delay = item.GetValue<int>("delay");
            if(code != null) {
                serviceBuilder.SetService(code, delay);
            }
        }

        return serviceBuilder.Build();
    }

}