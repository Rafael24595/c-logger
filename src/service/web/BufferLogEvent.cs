using System.Runtime.CompilerServices;
using System.Text.Json;

public class BufferLogEvent {

    private const string DIRECTORY = "./.temp/logs/";

    private readonly List<LogEvent> buffer;

    public BufferLogEvent() {
        this.buffer = [];
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BufferLogEvent Insert(LogEvent log) {
        this.buffer.Add(log);
        return this;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Optional<LogEvent> Take() {
        if(this.buffer.Count < 1) {
            return Optional<LogEvent>.None();
        }
        return Optional<LogEvent>.Some(this.buffer.First());
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BufferLogEvent LocalFlush() {
        if(this.buffer.Count < 1) {
            return this;
        }
        string json = JsonSerializer.Serialize(this.buffer);
        this.buffer.Clear();
        string name = $"Log-Buffer-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}.json";
        
        this.CreateDirectoryIfNotExists()
            .WritteFile(name, json);

        return this;
    }

    private BufferLogEvent CreateDirectoryIfNotExists() {
        if (!Directory.Exists(DIRECTORY)) {
            try {
                Directory.CreateDirectory(DIRECTORY);
                Console.WriteLine("Directory created successfully.");
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while creating the directory: {ex.Message}");
            }
        }
        return this;
    }

    private BufferLogEvent WritteFile(string name, string json) {
        try {
            using (StreamWriter writer = new($"{DIRECTORY}{name}")) {
                writer.Write(json);
            }
            Console.WriteLine("String written to file successfully.");
        } catch (Exception ex) {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return this;
    }

}