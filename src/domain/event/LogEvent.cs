using System.Text.Json;

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


    [Field("inserted_by", EFieldType.STRING, 50)]
    public string InsertedBy { get; set; }

    [Field("inserted_on", EFieldType.STRING, 50)]
    public string InsertedOn { get; set; }

    [Field("inserted_at", EFieldType.BIGINT)]
    public long InsertedAt { get; set; }


    public LogEvent() {
        this.Id = 0;
        this.Service = "";
        this.SessionId = "";
        this.Category = "";
        this.Family = "";
        this.Timestamp = 0;
        this.Message = "";
        this.InsertedBy = "";
        this.InsertedOn = "";
        this.InsertedAt = 0;
    }

    public LogEvent(string service, string sessionId, string category, string family, long timestamp, string message, string insertedBy, string insertedOn, long insertedAt) {
        this.Id = 0;
        this.Service = service;
        this.SessionId = sessionId;
        this.Category = category;
        this.Family = family;
        this.Timestamp = timestamp;
        this.Message = message;
        this.InsertedBy = insertedBy;
        this.InsertedOn = insertedOn;
        this.InsertedAt = insertedAt;
    }

    public static Optional<LogEvent> From(string log) {
        try {         
            return Optional<LogEvent>.SomeNullable(JsonSerializer.Deserialize<LogEvent>(log));   
        } catch (Exception) {
            return Optional<LogEvent>.None();
        }
    }

}