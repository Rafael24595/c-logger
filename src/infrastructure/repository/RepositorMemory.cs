public class RepositorMemory: IRepository {

    public const string NAME = "RepositorMemory";

    private readonly Dictionary<string, Dictionary<string, List<LogEvent>>> collection;

    public RepositorMemory(Persistence persistence) {
        this.collection = new();
    }
    
    private Result<List<LogEvent>, LogApiException> GetConnection(string service, string session) {
        if(!this.collection.ContainsKey(service)) {
            this.collection.Add(service, []);
        }
        
        var oService = this.collection[service];
        if(!oService.ContainsKey(session)) {
            oService.Add(session, []);
        }

        return Result<List<LogEvent>, LogApiException>.OK(oService[session]);
    }

    public bool Status() {
        return true;
    }

    public Result<LogEvent, LogApiException> Find(string id) {
        foreach (var services in this.collection) {
            foreach (var sessions in services.Value) {
                foreach (var item in sessions.Value) {
                    if(item.Id.Equals(id)) {
                        return Result<LogEvent, LogApiException>.OK(item);
                    }
                }
            }
        }

        LogApiException exception = new LogApiException(404, "", "Logs not found.");
        return Result<LogEvent, LogApiException>.ERR(exception);
    }

    public Result<List<LogEvent>, LogApiException> FindAll(string service, string session) {
        var oConnection = this.GetConnection(service, session);
        if(oConnection.IsErr()) {
            Optional<LogApiException> err = oConnection.Err();
            return Result<List<LogEvent>, LogApiException>.ERR(err.Unwrap());
        }

        var connection = oConnection.Ok().Unwrap();
        return Result<List<LogEvent>, LogApiException>.OK(connection);
    }

    public Result<LogEvent, LogApiException> Insert(LogEvent log) {
        var oConnection = this.GetConnection(log.Service, log.SessionId);
        if(oConnection.IsErr()) {
            Optional<LogApiException> err = oConnection.Err();
            return Result<LogEvent, LogApiException>.ERR(err.Unwrap());
        }

        var connection = oConnection.Ok().Unwrap();
        log.Id = connection.Count;
        connection.Add(log);

        return Result<LogEvent, LogApiException>.OK(log);
    }

}