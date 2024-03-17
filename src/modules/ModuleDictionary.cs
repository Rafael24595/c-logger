public class ModuleDictioary {

    public static Optional<IModule> LoadModule(string code, IConfigurationSection config) {
        var arguments = config.GetSection("arguments");
        switch (code) {
            case RustAuthMain.NAME:
                return Optional<IModule>.Some(new RustAuthMain(arguments));
            default: 
                var value = LoadCloseableModule(code, config);
                if(value.IsSome()) {
                    return Optional<IModule>.Some(value.Unwrap());
                }
                return Optional<IModule>.None();
        }
    }

    public static Optional<ICloseableModule> LoadCloseableModule(string code, IConfigurationSection config) {
        var arguments = config.GetSection("arguments");
        return code switch {
            KafkaMain.NAME => Optional<ICloseableModule>.Some(new KafkaMain(arguments)),
            _ => Optional<ICloseableModule>.None(),
        };
    }

}