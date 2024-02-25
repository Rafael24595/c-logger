public class Configuration {

    private readonly string[] webHandlers;
    private readonly (string, int)[] serviceEvents;

    internal Configuration(string[] webHandlers, (string, int)[] serviceEvents) {
        this.webHandlers = webHandlers;
        this.serviceEvents = serviceEvents;
    }

    public string[] WebHandlers() {
        return this.webHandlers;
    }

    public (string, int)[] ServiceEvents() {
        return this.serviceEvents;
    }
    
}