public class BuilderServiceWeb {

    private List<Func<LoggerRequest, LoggerRequest>> inputHandlers;
    private List<Func<LoggerResponse, LoggerResponse>> outputHandlers;

    public BuilderServiceWeb() {
        this.inputHandlers = [];
        this.outputHandlers = [];
    } 

    public BuilderServiceWeb SetHandler(string code) {
        var oHandler = ServiceWebHandlers.Find(code);
        if(oHandler.IsSome()) {
            var handler = oHandler.Unwrap();
            var input = handler.Item1;
            var output = handler.Item2;
            this.inputHandlers.Add(input);
            if(output.IsSome()) {
                this.outputHandlers.Add(output.Unwrap());
            }
        } else {
            Console.WriteLine("Not found: " + code);
        }
        return this;
    }

    public ServiceWeb Build() {
        return new([.. this.inputHandlers], [.. this.outputHandlers]);
    }

}