public class LoggerResponse {
    
    public static LoggerResponse From(LoggerResponse logger) {
        return new();
    }
    public LoggerResponse clone() {
        return LoggerResponse.From(this);
    }

}