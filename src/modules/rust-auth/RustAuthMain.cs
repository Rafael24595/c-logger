using System.Text.Json;

public class RustAuthMain: IModule {

    public const string NAME = "RustAuth";

    private readonly IConfigurationSection args;
    private readonly RustAuthResolver resolver;
    private Optional<string> token;
    private Optional<IManagerSymmetric> manager;
    private Optional<RustAuthHandler> Handler;

    public RustAuthMain(IConfigurationSection args) {
        this.args = args;
        this.resolver = new RustAuthResolver(this.args.GetValue<string>("host") ?? "");
        this.token = Optional<string>.None();
        this.manager = Optional<IManagerSymmetric>.None();
        this.Handler = Optional<RustAuthHandler>.None();
    }

    public Optional<LogConfigException> Initialize(WebApplication app, BuilderServiceWeb serviceBuilder) {
        this.InitializeManager();

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

    private Result<IManagerSymmetric, LogConfigException> InitializeManager() {
        var code = this.args.GetValue<string>("manager") ?? "";
        var manager = DictionaryManagerSymmetric.Find(code, this.args);
        if(manager.IsNone()) {
            var exception = new LogConfigException("", "Symmetric manager not found.");
            return Result<IManagerSymmetric, LogConfigException>.ERR(exception);
        }
        this.manager = manager;
        return Result<IManagerSymmetric, LogConfigException>.OK(manager.Unwrap());
    }

    private Result<RustAuthHandler, LogConfigException> InitializeHandler() {
        var status = manager.Unwrap().Status();
        if(status.IsSome()) {
            var exception = new LogConfigException("", status.Unwrap());
            return Result<RustAuthHandler, LogConfigException>.ERR(exception);
        }

        RustAuthHandler handler = new(this.manager.Unwrap());
        return Result<RustAuthHandler, LogConfigException>.OK(handler);
    }

    private Result<string, LogConfigException> Suscribe() {
        var rPubkey = Task.Run(this.resolver.PubKey).Result;
        if(rPubkey.IsErr()) {
            var exception = new LogConfigException("", rPubkey.Err().Unwrap().Message);
            return Result<string, LogConfigException>.ERR(exception);
        }
        
        var pubkey = rPubkey.Ok().Unwrap();
        var manager = DictionaryManagerAsymmetric.Find(pubkey.module);
        if(manager.IsNone()) {
            var exception = new LogConfigException("", "Asymmetric manager not found.");
            return Result<string, LogConfigException>.ERR(exception);
        }

        if(this.manager.IsNone()) {
            var exception = new LogConfigException("", "Symmetric manager not found.");
            return Result<string, LogConfigException>.ERR(exception);
        }

        var config = Configuration.GetInstance();
        if(config.IsNone()) {
            var exception = new LogConfigException("", "Configuration is not loaded.");
            return Result<string, LogConfigException>.ERR(exception);
        }

        var service = config.Unwrap().service;
        var pass = this.args.GetValue<string>("pass_key") ?? "";
        var key = this.manager.Unwrap().Key();
        var host = config.Unwrap().host;
        var request = new SuscribeRequest(service, pass, key, host, "status", "key");

        string json = JsonSerializer.Serialize(request);
        var encJson = manager.Unwrap().Encrypt(pubkey, json);

        var payload = new SuscribePayload(encJson);

        var rToken = Task.Run(() => this.resolver.Suscribe(payload)).Result;
        if(rToken.IsErr()) {
            var exception = new LogConfigException("", rToken.Err().Unwrap().Message);
            return Result<string, LogConfigException>.ERR(exception);
        }

        return Result<string, LogConfigException>.OK(rToken.Ok().Unwrap());
    }

}