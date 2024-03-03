public class LogApiException(int status, string code, string message) : Exception(message) {

    public int Status { get; } = status;
    public string Code { get; } = code;

    public LogApiException(int status, string code, Exception exception): this(status, code, exception.Message) {
    }
    
}