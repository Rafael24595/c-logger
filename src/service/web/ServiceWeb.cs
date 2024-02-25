public class ServiceWeb {

    private Func<LoggerRequest, LoggerRequest>[] handlers;

    internal ServiceWeb(Func<LoggerRequest, LoggerRequest>[] handlers) {
        this.handlers = handlers;
    }

    public LoggerResponse HelloWorld(LoggerRequest request) {
        request = this.ExecuteHandlers(request);
        return new();
    }

    private LoggerRequest ExecuteHandlers(LoggerRequest request) {
        LoggerRequest aux = request.clone();
        foreach (var handler in this.handlers) {
            aux = handler(aux);
        }
        return aux;
    }

}