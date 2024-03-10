public class LoggerRequest {

    private readonly string path;
    private readonly Dictionary<string, List<string>> headers;
    private readonly Dictionary<string, List<string>> query;
    private readonly Dictionary<string, string> param;
    public string Body { get; set; }

     public LoggerRequest(string path, Dictionary<string, List<string>> headers, 
                         Dictionary<string, List<string>> query, Dictionary<string, string> param, 
                         string body) {
        this.path = path;
        this.headers = headers ?? [];
        this.query = query ?? [];
        this.param = param ?? [];
        this.Body = body;
    }

    public static LoggerRequest From(LoggerRequest logger) {
        return new LoggerRequest(logger.path, new Dictionary<string, List<string>>(logger.headers),
                                 new Dictionary<string, List<string>>(logger.query),
                                 new Dictionary<string, string>(logger.param),
                                 logger.Body);
    }
    
    public LoggerRequest Clone() {
        return LoggerRequest.From(this);
    }

    public Optional<string> PathParam(string id) {
        if(!this.param.ContainsKey(id)) {
            return Optional<string>.None();
        }
        return Optional<string>.Some(this.param[id]);
    }

}