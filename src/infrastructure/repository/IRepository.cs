interface IRepository {
    Optional<LogEvent> Find(string id);
    Optional<List<LogEvent>> FindAll(string service, string session_id);
    Optional<LogEvent> Insert(LogEvent log);
}