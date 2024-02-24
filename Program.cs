static void start(string[] args) {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapGet("/hello-world", helloWolrd)
        .WithName("GetHelloWorld")
        .WithOpenApi();

    app.Run();
}

static IResult helloWolrd(HttpRequest request) {
    return Results.Ok("Hello World");
}

start(args);