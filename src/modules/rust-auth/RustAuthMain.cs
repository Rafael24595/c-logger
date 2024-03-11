public class RustAuthMain: IModule {

    public const string NAME = "RustAuth";

    private readonly IConfigurationSection args;
    private readonly RustAuthResolver resolver;
    private Optional<string> token;
    private Optional<RustAuthHandler> Handler;

    public RustAuthMain(IConfigurationSection args) {
        this.args = args;
        this.resolver = new RustAuthResolver(this.args.GetValue<string>("host") ?? "");
        this.token = Optional<string>.None();
        this.Handler = Optional<RustAuthHandler>.None();
    }

    public Optional<LogConfigException> Initialize(WebApplication app, BuilderServiceWeb serviceBuilder) {
        var token = this.Suscribe();
        if(token.IsErr()) {
            return token.Err();
        }

        this.token = token.Ok();

        ControllerRustAuth.Initialize(app);
        var rHandler = InitializeHandler();
        if(rHandler.IsErr()) {
            return rHandler.Err();
        }

        this.Handler = rHandler.Ok();
        var handler = this.Handler.Unwrap();

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
            return Result<RustAuthHandler, LogConfigException>.ERR(exception);
        }

        var status = manager.Unwrap().Status();
        if(status.IsSome()) {
            var exception = new LogConfigException("", status.Unwrap());
            return Result<RustAuthHandler, LogConfigException>.ERR(exception);
        }

        RustAuthHandler handler = new(manager.Unwrap());
        return Result<RustAuthHandler, LogConfigException>.OK(handler);
    }

    private Result<string, LogConfigException> Suscribe() {
        var rPubkey = Task.Run(this.resolver.PubKey).Result;
        var pubkey = rPubkey.Ok().Unwrap();
        var manager = DictionaryManagerAsymmetric.Find(pubkey.module);
        if(manager.IsNone()) {
            var exception = new LogConfigException("", "Asymmetric manager not found.");
            return Result<string, LogConfigException>.ERR(exception);
        }
        var request = new SuscribeRequest();
        return Result<string, LogConfigException>.OK("TODO: Get token.");
    }

}