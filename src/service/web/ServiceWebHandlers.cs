public class ServiceWebHandlers {

    public static Optional<Func<LoggerRequest, LoggerRequest>> Find(string code) {
        switch (code) {
            case "TestHandler":
                return Optional<Func<LoggerRequest, LoggerRequest>>.Some(TestHandler);
            default:
                return Optional<Func<LoggerRequest, LoggerRequest>>.None();
        }
    }

    public static LoggerRequest TestHandler(LoggerRequest request) {
        Console.WriteLine("Oh hey!");
        return request;
    }

}