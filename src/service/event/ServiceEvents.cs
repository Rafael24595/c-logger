public class ServiceEvents {

    public static Optional<Func<bool>> find(string code) {
        switch (code) {
            case "TestEvent":
                return Optional<Func<bool>>.Some(TestEvent);
            default:
                return Optional<Func<bool>>.None();
        }
    }

    public static bool TestEvent() {
        Console.WriteLine("Hello event!");
        return true;
    }

}