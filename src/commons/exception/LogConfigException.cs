public class LogConfigException(string code, string message) : Exception(message) {

    public string Code { get; } = code;

    public LogConfigException(string code, Exception exception): this(code, exception.Message) {
    }
    
}