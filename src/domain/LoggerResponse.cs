using System.Text;

public class LoggerResponse {
    
    public int Status { get; set; }
    public string Content { get; set; }
    public Encoding Encoding { get; set; }
    public string Body { get; set; }

    public LoggerResponse(): this(200, "text/plain", Encoding.UTF8, "") {
    }

    public LoggerResponse(int status, string content, Encoding encoding, string body) {
        this.Status = status;
        this.Content = content;
        this.Encoding = encoding;
        this.Body = body;
    }

    public static LoggerResponse From(LoggerResponse logger) {
        return new LoggerResponse(logger.Status, logger.Content, logger.Encoding, logger.Body);
    }
    public LoggerResponse clone() {
        return LoggerResponse.From(this);
    }

}