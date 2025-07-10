using Microsoft.Extensions.Options;

// set Database__MaxConnections=230
// set Timeout=300

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppConfig>(builder.Configuration);
builder.Services.AddSingleton<ConfigService>();

var app = builder.Build();

app.MapGet("/config", (IOptions<AppConfig> options) => options.Value);

app.Run();

public class AppConfig
{
    public string Environment { get; set; } = "Development";
    public int Timeout { get; set; } = 30;
    public DatabaseConfig Database { get; set; } = new();
}

public class DatabaseConfig
{
    public string ConnectionString { get; set; } = "default";
    public int MaxConnections { get; set; } = 100;
}

public class ConfigService
{
    private readonly IConfiguration config;
    public ConfigService(IConfiguration config)
    {
        this.config = config;
        Console.WriteLine($"Database: {config["Database:ConnectionString"]}");
    }

}
