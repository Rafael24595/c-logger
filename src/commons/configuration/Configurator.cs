public class Configurator {

    public static Configuration Build() {
        IConfigurationSection config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build()
            .GetSection("AppConfig");

        List<string> webHandlers = [];
        foreach (var item in config.GetSection("WebHandlers").GetChildren()) {
            if(item.Value != null) {
                webHandlers.Add(item.Value);
                Console.WriteLine(item.Value);
            }
        }

        List<(string, int)> serviceEvents = [];
        foreach (var item in config.GetSection("ServiceEvents").GetChildren()) {
            var code = item.GetValue<string>("code");
            var delay = item.GetValue<int>("delay");
            if(code != null) {
                serviceEvents.Add((code, delay));
            }
        }

        var persistence = config.GetSection("Persistence");
        var pCode = persistence.GetValue<string>("code");
        var pConnection = persistence.GetValue<string>("connection");
        var pDatabase = persistence.GetValue<string>("database");
        var pCollection = persistence.GetValue<string>("collection");

        return new([.. webHandlers], [.. serviceEvents], new(pCode, pConnection, pDatabase, pCollection));
    }

}