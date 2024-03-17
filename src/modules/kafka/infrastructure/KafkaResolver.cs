using System.Text.Json;
using Confluent.Kafka;

public class KafkaResolver {

    private readonly ConsumerConfig configuration;
    private readonly string topic;
    private bool listen;
    private EStatus status;

    public KafkaResolver(ConsumerConfig configuration, string topic) {
        this.configuration = configuration;
        this.topic = topic;
        this.listen = false;
        this.status = EStatus.STOPPED;
    }

    public EStatus Status() {
        return this.status;
    }

    public bool IsListening() {
        return this.listen;
    }

    public void Suscribe() {
        try {
            using var consumer = new ConsumerBuilder<Ignore, string>(this.configuration).Build();

            this.listen = true;
            this.status = EStatus.LISTEN;

            consumer.Subscribe(this.topic);

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true;
                cts.Cancel();
            };

            var exception = this.Listen(consumer, cts);
            if(exception.IsSome()) {
                var message = exception.Unwrap().Message;
                this.status = EStatus.FAILED;
                Console.WriteLine(exception.Unwrap().Message);
                return;
            }
            
            this.status = EStatus.STOPPED;
        } catch (Exception e) {
            this.status = EStatus.FAILED;
            Console.WriteLine(e.Message);
        }
    }

    private Optional<LogConfigException> Listen(IConsumer<Ignore, string> consumer, CancellationTokenSource cts) {
        try {
            while (this.listen) {
                try {
                    var message = consumer.Consume(cts.Token);

                    var config = Configuration.GetInstance();
                    if(config.IsNone()) {
                        var exception = new LogConfigException("", "Configuration is no defined.");
                        return Optional<LogConfigException>.Some(exception);
                    }

                    var repository = config.Unwrap().container.GetRepository();
                    if(config.IsNone()) {
                        var exception = new LogConfigException("", "Persistence repository is no defined.");
                        return Optional<LogConfigException>.Some(exception);
                    }

                    var logSerialized = message.Message.Value;
                    var oLog = LogEvent.From(logSerialized);
                    if(oLog.IsNone()) {
                        Console.WriteLine($"Log cannot be deserialized: {logSerialized}");
                        continue;
                    }

                    var log = Configuration.UpdateEventMisc(oLog.Unwrap());

                    repository.Unwrap().Insert(log);
                } catch (ConsumeException e) {
                    Console.WriteLine($"Error consuming message: {e.Error.Reason}");
                }
            }
        } catch (OperationCanceledException) {
            consumer.Close();
        }

        return Optional<LogConfigException>.None();
    }

    public Result<EStatus, LogConfigException> Close() {
        if(!this.listen) {
            var exception = new LogConfigException("", "Kafka service is already stopped");
            return  Result<EStatus, LogConfigException>.ERR(exception);
        }

        this.listen = false;
        this.status = EStatus.CLOSING;

        return Result<EStatus, LogConfigException>.OK(this.status);
    }

}