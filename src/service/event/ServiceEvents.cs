public class ServiceEvents {

    internal static Optional<Func<bool>> Find(string code) {
        switch (code) {
            case "TestEvent":
                return Optional<Func<bool>>.Some(TestEvent);
            default:
                return Optional<Func<bool>>.None();
        }
    }

    internal static bool TestEvent() {
        Console.WriteLine("Hello event!");
        return true;
    }

}