public class Configurator {

    public static Configuration Build() {
        IConfigurationSection config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build()
            .GetSection("AppConfig");

        string host = config.GetValue<string>("Host") ?? "";
        string service = config.GetValue<string>("Service") ?? "";
        List<IModule> modules = LoadModules(config);
        Persistence persistence = LoadPersistence(config);
        Optional<IRepository> repository = RepositoryDictionary.Find(persistence);
        ServiceManagerEvents events = LoadServiceEvent(config);

        DependencyContainer container = LoadDependencyContainer(events, repository);

        return Configuration.Initialize(service, host, container, [.. modules], persistence);
    }

    private static List<IModule> LoadModules(IConfigurationSection config) {
        List<IModule> modules = [];
         foreach (var item in config.GetSection("Modules").GetChildren()) {
            var active = item.GetValue<bool>("active");
            if(active) {
                var code = item.GetValue<string>("code") ?? "";
                var module = LoadModule(code, item);
                if(module.IsSome()) {
                    modules.Add(module.Unwrap());
                }
            }
        }
        return modules;
    }

    private static Optional<IModule> LoadModule(string code, IConfigurationSection config) {
        var arguments = config.GetSection("arguments");
        return code switch {
            RustAuthMain.NAME => Optional<IModule>.Some(new RustAuthMain(arguments)),
            _ => Optional<IModule>.None(),
        };
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
        BuilderServiceManagerEvents serviceBuilder = new();

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