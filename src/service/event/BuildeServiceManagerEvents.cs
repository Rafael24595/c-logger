public class BuilderServiceManagerEvents {

    private List<ServiceEvent> services;

    public BuilderServiceManagerEvents() {
        this.services = [];
    } 

    public BuilderServiceManagerEvents SetService(string code, int delay) {
        var serviceEvent = ServiceEvents.Find(code);
        
        if(serviceEvent.IsSome()) {
            ServiceEvent service = new(serviceEvent.Unwrap(), delay);
            this.services.Add(service);
        } else {
            Console.WriteLine("Not found: " + code);
        }
        
        return this;
    }

    public ServiceManagerEvents Build() {
        return new([.. this.services]);
    }

}