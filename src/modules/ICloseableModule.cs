public interface ICloseableModule: IModule {
     public EStatus Status();
     public Optional<LogConfigException> Launch();
     public Result<EStatus, LogConfigException> Close();
}