using _12_service_mw.Middleware;
using _12_service_mw.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITimeService, TimeService>();
builder.Services.AddTransient<IRequestCounter, RequestCounter>();      // :TODO: ???
builder.Services.AddLogging(logging =>
    logging.AddConsole().SetMinimumLevel(LogLevel.Debug)
);

var app = builder.Build();

app.UseMiddleware<ServiceInjectionMiddleware>();
app.UseMiddleware<ScopedServiceMiddleware>();
app.UseMiddleware<AuthMiddleware>();

app.MapGet("/", (HttpContext ctx) =>
{
    int reqNumber = (int)ctx.Items["RequestNumber"];
    return $"Request #{reqNumber} at {ctx.Response.Headers["X-Server-Time"]}";
});

app.MapGet("/secure", (HttpContext ctx) =>
{
    int reqNumber = (int)ctx.Items["RequestNumber"];
    return $"Secure request #{reqNumber}";
});

app.Run();
