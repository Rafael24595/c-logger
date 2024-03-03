public class LogApiException: Exception {

    private readonly long status;
    private readonly string code;

    public LogApiException(long status, string code, string message): base(message) {
        this.status = status;
        this.code = code;
    }

    public LogApiException(long status, string code, Exception exception): base(exception.Message) {
        this.status = status;
        this.code = code;
    }

}