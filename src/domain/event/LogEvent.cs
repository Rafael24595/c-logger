public class LogEvent {

    [Field("id", EFieldKey.IS_KEY, EFieldType.INTEGER)]
    public int Id { get; set; }

    [Field("service", EFieldType.STRING, 50)]
    public string Service { get; set; }

    [Field("session_id", EFieldType.STRING, 50)]
    public string SessionId { get; set; }

    [Field("category", EFieldType.STRING, 50)]
    public string Category { get; set; }

    [Field("family", EFieldType.STRING, 50)]
    public string Family { get; set; }

    [Field("timestamp", EFieldType.BIGINT)]
    public long Timestamp { get; set; }

    [Field("message", EFieldType.STRING)]
    public string Message { get; set; }

    public LogEvent() {
        this.Id = 0;
        this.Service = "";
        this.SessionId = "";
        this.Category = "";
        this.Family = "";
        this.Timestamp = 0;
        this.Message = "";
    }

    public LogEvent( string service, string sessionId, string category, string family, long timestamp, string message) {
        this.Id = 0;
        this.Service = service;
        this.SessionId = sessionId;
        this.Category = category;
        this.Family = family;
        this.Timestamp = timestamp;
        this.Message = message;
    }

}