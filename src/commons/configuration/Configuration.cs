public class Configuration {

    public readonly DependencyContainer container;
    public readonly string[] webHandlers;
    public readonly Persistence persistence;

    internal Configuration(DependencyContainer container, string[] webHandlers, Persistence persistence) {
        this.webHandlers = webHandlers;
        this.container = container;
        this.persistence = persistence;
    }
    
}