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
        List<ICloseableModule> closeableModules = LoadCloseableModules(config);
        Persistence persistence = LoadPersistence(config);
        Optional<IRepository> repository = RepositoryDictionary.Find(persistence);

        DependencyContainer container = LoadDependencyContainer(repository);

        return Configuration.Initialize(service, host, container, [.. modules], [.. closeableModules], persistence);
    }

    private static List<IModule> LoadModules(IConfigurationSection config) {
        List<IModule> modules = [];
         foreach (var item in config.GetSection("Modules").GetSection("wrapper").GetChildren()) {
            var active = item.GetValue<bool>("active");
            if(active) {
                var code = item.GetValue<string>("code") ?? "";
                var module = ModuleDictioary.LoadModule(code, item);
                if(module.IsSome()) {
                    modules.Add(module.Unwrap());
                }
            }
        }
        return modules;
    }

    private static List<ICloseableModule> LoadCloseableModules(IConfigurationSection config) {
        List<ICloseableModule> modules = [];
         foreach (var item in config.GetSection("Modules").GetSection("closeable").GetChildren()) {
            var active = item.GetValue<bool>("active");
            if(active) {
                var code = item.GetValue<string>("code") ?? "";
                var module = ModuleDictioary.LoadCloseableModule(code, item);
                if(module.IsSome()) {
                    modules.Add(module.Unwrap());
                }
            }
        }
        return modules;
    }

     private static Persistence LoadPersistence(IConfigurationSection config) {
        var persistence = config.GetSection("Persistence");
        
        var pCode = persistence.GetValue<string>("code") ?? "";
        var pConnection = persistence.GetValue<string>("connection") ?? "";
        var pDatabase = persistence.GetValue<string>("database") ?? "";
        var pCollection = persistence.GetValue<string>("collection") ?? "";

        return new(pCode, pConnection, pDatabase, pCollection);
    }

    private static DependencyContainer LoadDependencyContainer(Optional<IRepository> repository) {
        DependencyContainer container = new DependencyContainer()
            .SetRepository(repository);

        return container;
    }

}