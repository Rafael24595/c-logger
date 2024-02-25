public class ServiceEvent {

    private readonly Func<bool> service;
    private readonly int delay;
    private long last;

    public ServiceEvent(Func<bool> service, int delay) {
        this.service = service;
        this.delay = delay;
        this.last = 0;
    }

    public bool CanExecute() {
        long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        return this.last + delay < milliseconds;
    }

    public bool Execute() {
        this.last = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        return this.service();
    }

}