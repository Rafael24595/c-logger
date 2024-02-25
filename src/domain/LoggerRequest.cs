public class LoggerRequest {

    public static LoggerRequest From(LoggerRequest logger) {
        return new();
    }
    public LoggerRequest clone() {
        return LoggerRequest.From(this);
    }

}