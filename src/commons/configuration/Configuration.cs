public class Configuration {

    public readonly string[] webHandlers;
    public readonly (string, int)[] serviceEvents;
    public readonly Persistence persistence;

    internal Configuration(string[] webHandlers, (string, int)[] serviceEvents, Persistence persistence) {
        this.webHandlers = webHandlers;
        this.serviceEvents = serviceEvents;
        this.persistence = persistence;
    }
    
}