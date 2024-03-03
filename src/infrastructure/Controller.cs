public class Controller {
    
    private ServiceWeb service;

    private Controller(ServiceWeb service) {
        this.service = service;
    }

    public static WebApplication Initialize(WebApplication app, ServiceWeb service) {
        Controller controller = new(service);

        app.MapPost("/insert", controller.Insert)
            .WithName("PostInsert")
            .WithOpenApi();

        app.MapGet("/find/{id}", controller.Find)
            .WithName("GetFind")
            .WithOpenApi();

        app.MapGet("/find-all/{service}/{session}", controller.FindAll)
            .WithName("GetFindAll")
            .WithOpenApi();

        return app;
    }

    private async Task<IResult> Insert(HttpRequest request) {
        LoggerRequest logger = await RequestToLoggerRequest(request);
        LoggerResponse response = this.service.Insert(logger);
        return LoggerRequestToIResult(response);
    }

    private async Task<IResult> Find(HttpRequest request, string id) {
        Dictionary<string, string> param = new() {
            { "id", id }
        };
        LoggerRequest logger = await RequestToLoggerRequest(request, param);
        LoggerResponse response = this.service.Find(logger);
        return LoggerRequestToIResult(response);
    }

    private async Task<IResult> FindAll(HttpRequest request, string service, string session) {
        Dictionary<string, string> param = new() {
            { "service", service },
            { "session", session },
        };
        LoggerRequest logger = await RequestToLoggerRequest(request, param);
        LoggerResponse response = this.service.FindAll(logger);
        return LoggerRequestToIResult(response);
    }

    private async Task<LoggerRequest> RequestToLoggerRequest(HttpRequest request) {
        return await RequestToLoggerRequest(request, []);
    }

    private async Task<LoggerRequest> RequestToLoggerRequest(HttpRequest request, Dictionary<string, string> param) {
        string path = request.Path;

        Dictionary<string, List<string>> headers = new();
        foreach (var item in request.Headers.ToDictionary()) {
            headers.Add(item.Key, [.. item.Value]);
        }

        Dictionary<string, List<string>> query = new();
        foreach (var item in request.Query.AsEnumerable()) {
            headers.Add(item.Key, [.. item.Value]);
        }
        
        string body = "";
        if(!request.Method.Equals("GET")) {
            using StreamReader reader = new StreamReader(request.Body);
            body = await reader.ReadToEndAsync();
        }

        return new(path, headers, query, param, body);
    }

    private IResult LoggerRequestToIResult( LoggerResponse response) {
        return Results.Content(response.Body, response.Content, response.Encoding, response.Status);
    }

}