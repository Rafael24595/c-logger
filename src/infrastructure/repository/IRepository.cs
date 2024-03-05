public interface IRepository {
    bool Status();
    Result<LogEvent, LogApiException> Find(string id);
    Result<List<LogEvent>, LogApiException> FindAll(string service, string session_id);
    Result<LogEvent, LogApiException> Insert(LogEvent log);
}