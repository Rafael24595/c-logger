public class Configuration {

    public static Optional<Configuration> Instance = Optional<Configuration>.None();

    public readonly string service;
    public readonly string host;
    public readonly DependencyContainer container;
    public readonly IModule[] modules;
    public readonly Persistence persistence;

    private Configuration(string service, string host, DependencyContainer container, IModule[] modules, Persistence persistence) {
        this.service = service;
        this.host = host;
        this.modules = modules;
        this.container = container;
        this.persistence = persistence;
    }

    public static Configuration Initialize(string service, string host, DependencyContainer container, IModule[] modules, Persistence persistence) {
        if(Instance.IsNone()) {
            var instance = new Configuration(service, host, container, modules, persistence);
            Instance = Optional<Configuration>.Some(instance);
        }
        return Instance.Unwrap();
    }

    public static Optional<Configuration> GetInstance() {
        return Instance;
    }
    
}