
using System.Runtime.CompilerServices;
using Confluent.Kafka;

public class KafkaMain: ICloseableModule {

    public const string NAME = "Kafka";

    private readonly IConfigurationSection args;
    private ConsumerConfig configuration;
    private Optional<Thread> thread;
    private Optional<KafkaResolver> resolver;

    public KafkaMain(IConfigurationSection args) {
        this.args = args;
        this.configuration = new ConsumerConfig {
            GroupId = this.args.GetValue<string>("group_id") ?? "",
            BootstrapServers = this.args.GetValue<string>("host") ?? "",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        this.thread = Optional<Thread>.None();
        this.resolver = Optional<KafkaResolver>.None();
    }

    public Optional<LogConfigException> Initialize(WebApplication app, BuilderServiceWeb serviceBuilder) {
        return this.Launch();
    }

    public EStatus Status() {
        if(this.resolver.IsNone()) {
            return EStatus.NON_INITILIZED;
        }
        return this.resolver.Unwrap().Status();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Optional<LogConfigException> Launch() {
        if(this.resolver.IsSome() && this.resolver.Unwrap().IsListening()) {
            var exception = new LogConfigException("", "Kafka service is already listening");
            return Optional<LogConfigException>.Some(exception);
        }

        var topic = this.args.GetValue<string>("topic") ?? "";
        if(topic.Equals("")) {
            var exception = new LogConfigException("", "TOPIC IS UNDEFINED");
            return Optional<LogConfigException>.Some(exception);
        }

        var resolver = new KafkaResolver(this.configuration, topic);
        this.resolver = Optional<KafkaResolver>.Some(resolver);

        try {
            this.thread = Optional<Thread>.Some(new(this.resolver.Unwrap().Suscribe));
            this.thread.Unwrap().Start();
        } catch (Exception e){
            var exception = new LogConfigException("", e.Message);
            return Optional<LogConfigException>.Some(exception);
        }

        return Optional<LogConfigException>.None();
    }
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Result<EStatus, LogConfigException> Close() {
        if(this.resolver.IsNone()) {
            var exception = new LogConfigException("", "Kafka service is not initilized.");
            return  Result<EStatus, LogConfigException>.ERR(exception);
        }

        return this.resolver.Unwrap().Close();
    }

}