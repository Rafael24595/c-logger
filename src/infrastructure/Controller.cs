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

        app.MapGet("/find", controller.Find)
            .WithName("GetFind")
            .WithOpenApi();

        app.MapGet("/find-all", controller.FindAll)
            .WithName("GetFindAll")
            .WithOpenApi();

        return app;
    }

    private IResult Insert(HttpRequest request) {
        this.service.Insert(new());
        return Results.Ok("Hello World");
    }

    private IResult Find(HttpRequest request) {
        this.service.Insert(new());
        return Results.Ok("Hello World");
    }

    private IResult FindAll(HttpRequest request) {
        this.service.Insert(new());
        return Results.Ok("Hello World");
    }

}