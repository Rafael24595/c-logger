public class ServiceWeb {

    private Func<LoggerRequest, LoggerRequest>[] inputHandlers;
    private Func<LoggerResponse, LoggerResponse>[] outputHandlers;

    internal ServiceWeb(Func<LoggerRequest, LoggerRequest>[] inputHandlers, Func<LoggerResponse, LoggerResponse>[] outputHandlers) {
        this.inputHandlers = inputHandlers;
        this.outputHandlers = outputHandlers;
    }

    public LoggerResponse HelloWorld(LoggerRequest request) {
        request = this.ExecuteInputHandlers(request);
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