public class ServiceWeb {

    private readonly IRepository repository;
    private Func<LoggerRequest, LoggerRequest>[] inputHandlers;
    private Func<LoggerResponse, LoggerResponse>[] outputHandlers;

    internal ServiceWeb( IRepository repository, Func<LoggerRequest, LoggerRequest>[] inputHandlers, Func<LoggerResponse, LoggerResponse>[] outputHandlers) {
        this.repository = repository;
        this.inputHandlers = inputHandlers;
        this.outputHandlers = outputHandlers;
    }

    public LoggerResponse Insert(LoggerRequest request) {
        request = this.ExecuteInputHandlers(request);
        LogEvent log = new LogEvent("TestService", "001", "TEST", "SELF", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond, "This is a test message");
        this.repository.Insert(log);
        LoggerResponse response = new();
        return this.ExecuteOutputHandlers(response);
    }

    public LoggerResponse Find(LoggerRequest request) {
        request = this.ExecuteInputHandlers(request);
        this.repository.Find("1");
        LoggerResponse response = new();
        return this.ExecuteOutputHandlers(response);
    }

    public LoggerResponse FindAll(LoggerRequest request) {
        request = this.ExecuteInputHandlers(request);
        this.repository.FindAll("TestService", "001");
        LoggerResponse response = new();
        return this.ExecuteOutputHandlers(response);
    }

    private LoggerRequest ExecuteInputHandlers(LoggerRequest request) {
        LoggerRequest aux = request.clone();
        foreach (var handler in this.inputHandlers) {
            aux = handler(aux);
        }
        return aux;
    }

    private LoggerResponse ExecuteOutputHandlers(LoggerResponse response) {
        LoggerResponse aux = response.clone();
        foreach (var handler in this.outputHandlers) {
            aux = handler(aux);
        }
        return aux;
    }

}