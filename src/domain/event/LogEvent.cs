public class LogEvent {

    [Field("id", EFieldKey.IS_KEY, EFieldType.INTEGER)]
    public int Id { get; }

    [Field("service", EFieldType.STRING, 50)]
    public string Service { get; }

    [Field("session_id", EFieldType.STRING, 50)]
    public string SessionId { get; }

    [Field("category", EFieldType.STRING, 50)]
    public string Category { get; }

    [Field("family", EFieldType.STRING, 50)]
    public string Family { get; }

    [Field("timestamp", EFieldType.INTEGER)]
    public long Timestamp { get; }

    [Field("message", EFieldType.STRING)]
    public string Message { get; }

}