public class BuilderServiceWeb {

    private readonly IRepository repository;
    private readonly List<Func<LoggerRequest, LoggerRequest>> inputHandlers;
    private readonly List<Func<LoggerResponse, LoggerResponse>> outputHandlers;

    public BuilderServiceWeb(IRepository repository) {
        this.repository = repository;
        this.inputHandlers = [];
        this.outputHandlers = [];
    } 

    public BuilderServiceWeb SetHandler((Func<LoggerRequest, LoggerRequest>, Optional<Func<LoggerResponse, LoggerResponse>>) handler) {
        var input = handler.Item1;
        var output = handler.Item2;
        this.inputHandlers.Add(input);
        if(output.IsSome()) {
            this.outputHandlers.Add(output.Unwrap());
        }
        return this;
    }

    public ServiceWeb Build() {
        return new(repository, [.. this.inputHandlers], [.. this.outputHandlers]);
    }

}