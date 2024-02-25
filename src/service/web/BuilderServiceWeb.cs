public class BuilderServiceWeb {

    private List<Func<LoggerRequest, LoggerRequest>> handlers;

    public BuilderServiceWeb() {
        this.handlers = [];
    } 

    public BuilderServiceWeb SetHandler(string code) {
        var handler = ServiceWebHandlers.Find(code);
        
        if(handler.IsSome()) {
            this.handlers.Add(handler.Unwrap());
        } else {
            Console.WriteLine("Not found: " + code);
        }
        
        return this;
    }
    public BuilderServiceWeb SetHandler(Func<LoggerRequest, LoggerRequest> handler) {
        this.handlers.Add(handler);
        return this;
    }

    public ServiceWeb Build() {
        return new([.. this.handlers]);
    }

}