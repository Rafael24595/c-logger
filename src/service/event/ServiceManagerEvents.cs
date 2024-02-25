public class ServiceManagerEvents {

    private bool status;
    private readonly Thread thread;
    private readonly ServiceEvent[] services;

    internal ServiceManagerEvents(ServiceEvent[] services) {
        this.status = false;
        this.services = services;
        this.thread = new(Listen);
    }

    public void Execute() {
        if(this.status) {
            return;
        }
        this.status = true;
        this.thread.Start();
    }

    public void Stop() {
        if(this.status) {
            return;
        }
        this.status = false;
        this.thread.Interrupt();
    }

    private void Listen() {
        this.status = true;
        while (this.status) {
            foreach (var service in this.services) {
                if(service.CanExecute()) {
                    service.Execute();
                }
            }
        }
    }

}