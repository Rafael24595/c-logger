public class BuilderServiceManagerEvents {

    private List<ServiceEvent> services;

    public BuilderServiceManagerEvents() {
        this.services = [];
    } 

    public BuilderServiceManagerEvents SetService(string code, int delay) {
        var serviceEvent = ServiceEvents.find(code);
        
        if(serviceEvent.IsSome()) {
            ServiceEvent service = new(serviceEvent.Unwrap(), delay);
            this.services.Add(service);
        } else {
            Console.WriteLine("Not found: " + code);
        }
        
        return this;
    }
    public BuilderServiceManagerEvents SetService(ServiceEvent service) {
        this.services.Add(service);
        return this;
    }

    public ServiceManagerEvents Build() {
        return new([.. this.services]);
    }

}