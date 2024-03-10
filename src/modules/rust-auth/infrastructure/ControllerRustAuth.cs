public class ControllerRustAuth {

    private ControllerRustAuth() {
    }

    public static WebApplication Initialize(WebApplication app) {
        ControllerRustAuth controller = new();

        app.MapGet("/status", controller.Status)
            .WithName("RustAuth_Status")
            .WithOpenApi();

        app.MapGet("/key", controller.Key)
            .WithName("RustAuth_Key")
            .WithOpenApi();

        return app;
    }

    private IResult Status(HttpRequest request) {
        return Results.Ok();
    }

    private IResult Key(HttpRequest request, string id) {
        //TODO: Return public key.
        return Results.Ok();
    }

}