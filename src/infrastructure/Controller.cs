public class Controller {
    
    private ServiceWeb service;

    private Controller(ServiceWeb service) {
        this.service = service;
    }

    public static WebApplication Initialize(WebApplication app, ServiceWeb service) {
        Controller controller = new(service);

        app.MapGet("/hello-world", controller.HelloWolrd)
            .WithName("GetHelloWorld")
            .WithOpenApi();

        return app;
    }

    private IResult HelloWolrd(HttpRequest request) {
        this.service.HelloWorld(new());
        return Results.Ok("Hello World");
    }

}