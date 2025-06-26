using _14_background_service.BackgroundServices;
using _14_background_service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDataProcessor, DataProcessor>();
builder.Services.AddSingleton<BackgroundWorker>();

var app = builder.Build();

// Start backgroundWorker
app.Services.GetRequiredService<BackgroundWorker>();

app.MapGet("/", () => "Backround service is running");

app.Run();
