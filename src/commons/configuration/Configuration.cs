public class Configuration {

    public readonly DependencyContainer container;
    public readonly IModule[] modules;
    public readonly Persistence persistence;

    internal Configuration(DependencyContainer container, IModule[] modules, Persistence persistence) {
        this.modules = modules;
        this.container = container;
        this.persistence = persistence;
    }
    
}