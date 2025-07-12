using _05_custom_provider;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddTextFile("config.txt");

builder.Services.Configure<AppSettings>(builder.Configuration);

var app = builder.Build();

// IOptions<AppSettings> --> SINGLETON
//app.MapGet("/config", (IOptions<AppSettings> options) =>
//{
//    var settings = options.Value;

//    return Results.Ok(settings);
//});

// IOptionsSnapshot<AppSettings> options --> SCOPED
//app.MapGet("/config", (IOptionsSnapshot<AppSettings> options) =>
//{
//    var settings = options.Value;

//    return Results.Ok(settings);
//});


app.MapGet("/config", (IOptionsMonitor<AppSettings> monitor) =>
{
    var settings = monitor.CurrentValue;

    monitor.OnChange(updatedSettings =>
    {
        Console.WriteLine("Config updated");
        settings = updatedSettings;
    });

    return Results.Ok(settings);
});


app.Run();


public class AppSettings
{
    public string AppName { get; set; } = string.Empty;
    public int MaxConnections { get; set; }
    public bool EnableLogging { get; set; }
    public string ApiKey { get; set; } = string.Empty;
    public TimeSpan Timeout { get; set; }
}
