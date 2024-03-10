public class RustAuthMain: IModule {

    public const string NAME = "RustAuth";

    private readonly IConfigurationSection args;

    public RustAuthMain(IConfigurationSection args) {
        this.args = args;
    }

    public Optional<LogConfigException> Initialize(WebApplication app, BuilderServiceWeb serviceBuilder) {
        ControllerRustAuth.Initialize(app);
        var rHandler = InitializeHandler();
        if(rHandler.IsErr()) {
            return rHandler.Err();
        }

        var handler = rHandler.Ok().Unwrap();

        var input = handler.InputFromAesGcmHandler;
        var output = Optional<Func<LoggerResponse, LoggerResponse>>.Some(handler.OutputFromAesGcmHandler);
        serviceBuilder.SetHandler((input, output));

        return Optional<LogConfigException>.None();
    }

    private Result<RustAuthHandler, LogConfigException> InitializeHandler() {
        var code = this.args.GetValue<string>("manager") ?? "";
        var manager = DictionaryManagerSymmetric.Find(code, this.args);
        if(manager.IsNone()) {
            var exception = new LogConfigException("", "Symmetric manager not found.");
            Result<RustAuthHandler, LogConfigException>.ERR(exception);
        }

        var status = manager.Unwrap().Status();
        if(status.IsSome()) {
            var exception = new LogConfigException("", status.Unwrap());
            Result<RustAuthHandler, LogConfigException>.ERR(exception);
        }

        RustAuthHandler handler = new(manager.Unwrap());
        return Result<RustAuthHandler, LogConfigException>.OK(handler);
    }

}