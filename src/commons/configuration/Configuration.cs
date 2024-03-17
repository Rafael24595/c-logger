public class Configuration {

    private const string DEFAULT_SERVICE = "GenericLog";
    private const string DEFAULT_SESSION = "00000000-0000-0000-0000-000000000000";

    public static Optional<Configuration> Instance = Optional<Configuration>.None();

    public readonly string service;
    public readonly string session;
    public readonly string host;
    public readonly DependencyContainer container;
    public readonly IModule[] modules;
    public readonly ICloseableModule[] closeableModules;
    public readonly Persistence persistence;

    private Configuration(string service, string session, string host, DependencyContainer container, IModule[] modules, ICloseableModule[] closeableModules, Persistence persistence) {
        this.service = service;
        this.session = session;
        this.host = host;
        this.modules = modules;
        this.closeableModules = closeableModules;
        this.container = container;
        this.persistence = persistence;
    }

    public static Configuration Initialize(string service, string session, string host, DependencyContainer container, IModule[] modules, ICloseableModule[] closeableModules, Persistence persistence) {
        if(Instance.IsNone()) {
            var instance = new Configuration(service, session, host, container, modules, closeableModules, persistence);
            Instance = Optional<Configuration>.Some(instance);
        }
        return Instance.Unwrap();
    }

    public static Optional<Configuration> GetInstance() {
        return Instance;
    }

    public DependencyContainer GetDependencyContainer() {
        return this.container;
    }

    public static LogEvent UpdateEventMisc(LogEvent log) {
        var service = DEFAULT_SERVICE;
        var session = DEFAULT_SESSION;

        if(Instance.IsSome()) {
            service = Instance.Unwrap().service;
            session = Instance.Unwrap().session;
        }

        log.InsertedBy = service;
        log.InsertedOn = session;
        log.InsertedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        return log;
    }
    
}