public interface IModule {
     public Optional<LogConfigException> Initialize(WebApplication app, BuilderServiceWeb serviceBuilder);
}