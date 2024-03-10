class RustAuthHandler {

    private readonly IManagerSymmetric manager;

    public RustAuthHandler(IManagerSymmetric manager) {
        this.manager = manager;
    }

    public LoggerRequest InputFromAesGcmHandler(LoggerRequest request) {
        Console.WriteLine("Input: Oh hey!");
        return request;
    }

    public  LoggerResponse OutputFromAesGcmHandler(LoggerResponse response) {
        Console.WriteLine("Output: Oh hey!");
        return response;
    }

}
