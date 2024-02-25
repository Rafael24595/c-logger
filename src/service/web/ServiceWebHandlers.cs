public class ServiceWebHandlers {

    internal static Optional<(Func<LoggerRequest, LoggerRequest>, Optional<Func<LoggerResponse, LoggerResponse>>)> Find(string code) {
        switch (code) {
            case "TestHandler":
                var input = InputTestHandler;
                var output = Optional<Func<LoggerResponse, LoggerResponse>>.Some(OutputTestHandler);
                return Optional<(Func<LoggerRequest, LoggerRequest>, Optional<Func<LoggerResponse, LoggerResponse>>)>.Some((input, output));
            default:
                return Optional<(Func<LoggerRequest, LoggerRequest>, Optional<Func<LoggerResponse, LoggerResponse>>)>.None();
        }
    }

    internal static LoggerRequest InputTestHandler(LoggerRequest request) {
        Console.WriteLine("Input: Oh hey!");
        return request;
    }

    internal static LoggerResponse OutputTestHandler(LoggerResponse response) {
        Console.WriteLine("Output: Oh hey!");
        return response;
    }

}