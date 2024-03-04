using System.Text.Json;

public class ServiceWeb {

    private readonly IRepository repository;
    private Func<LoggerRequest, LoggerRequest>[] inputHandlers;
    private Func<LoggerResponse, LoggerResponse>[] outputHandlers;
    private readonly BufferLogEvent buffer;

    internal ServiceWeb( IRepository repository, Func<LoggerRequest, LoggerRequest>[] inputHandlers, Func<LoggerResponse, LoggerResponse>[] outputHandlers) {
        this.repository = repository;
        this.inputHandlers = inputHandlers;
        this.outputHandlers = outputHandlers;
        this.buffer = new();
    }

    public LoggerResponse Insert(LoggerRequest request) {
        request = this.ExecuteInputHandlers(request);
        LoggerResponse response = new();

        Optional<LogEvent> log;
        try {         
            log = Optional<LogEvent>.Some(JsonSerializer.Deserialize<LogEvent>(request.Body));   
        } catch (Exception) {
            log = Optional<LogEvent>.None();
        }

        if(log.IsNone()) {
            response.Status = 400;
            return this.ExecuteOutputHandlers(response);
        }

        Result<LogEvent, LogApiException> logResult = this.repository.Insert(log.Unwrap());
        if(logResult.IsErr()) {
            this.buffer.Insert(log.Unwrap());
            var err = logResult.Err().Unwrap();
            response.Status = err.Status;
            response.Body = err.Message;
            return this.ExecuteOutputHandlers(response);
        }

        response.Body = JsonSerializer.Serialize(logResult.Ok().Unwrap());
        response.Content = "application/json";

        return this.ExecuteOutputHandlers(response);
    }

    public LoggerResponse Find(LoggerRequest request) {
        request = this.ExecuteInputHandlers(request);
        LoggerResponse response = new();

        Optional<string> id = request.PathParam("id");
        if(id.IsNone()) {
            response.Status = 400;
            return this.ExecuteOutputHandlers(response);
        }

        Result<LogEvent, LogApiException> log = this.repository.Find(id.Unwrap());
        if(log.IsErr()) {
            var err = log.Err().Unwrap();
            response.Status = err.Status;
            response.Body = err.Message;
            return this.ExecuteOutputHandlers(response);
        }

        response.Body = JsonSerializer.Serialize(log.Ok().Unwrap());
        response.Content = "application/json";

        return this.ExecuteOutputHandlers(response);
    }

    public LoggerResponse FindAll(LoggerRequest request) {
        request = this.ExecuteInputHandlers(request);

        LoggerResponse response = new();

        Optional<string> service = request.PathParam("service");
        Optional<string> session = request.PathParam("session");
        if(service.IsNone() || session.IsNone()) {
            response.Status = 400;
            return this.ExecuteOutputHandlers(response);
        }

        Result<List<LogEvent>, LogApiException> logs = this.repository.FindAll(service.Unwrap(), session.Unwrap());
        if(logs.IsErr()) {
            var err = logs.Err().Unwrap();
            response.Status = err.Status;
            response.Body = err.Message;
            return this.ExecuteOutputHandlers(response);
        }

        response.Body = JsonSerializer.Serialize(logs.Ok().Unwrap());
        response.Content = "application/json";

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