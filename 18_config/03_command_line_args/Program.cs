var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AppInfo>();

var app = builder.Build();

// TODO: ????
//app.MapGet("/", (AppInfo appInfo)
//    => $"App: {appInfo.Name}, Version: {appInfo.Version}, Debug: {appInfo.DebugMode}"
//);

app.MapGet("/", (IConfiguration c)
    => $"App: {c["name"]}, Version: {c["ver"]}, Debug: {c["debug"]}"
);

app.Run();

class AppInfo
{
    [ConfigurationKeyName("name")]
    public string Name { get; set; } = "DefaultApp";
    [ConfigurationKeyName("ver")]
    public string Version { get; set; } = "1.0";
    [ConfigurationKeyName("debug")]
    public bool DebugMode { get; set; }
}
