// using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


// ------------------

// index.html->index.htm-> default.html-> default.htm
//app.UseDefaultFiles();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseEndpoints(endpoints => { });

//app.MapGet("/", () => "Hello World!");
//app.Run();

// -------------------
//app.UseStaticFiles(new StaticFileOptions
//{
//    ServeUnknownFileTypes = true,
//    DefaultContentType = "application/octet-stream",
//    OnPrepareResponse = ctx =>
//    {
//        ctx.Context.Response.Headers.Append("X-Test", "Vasia");
//    }

//});

//app.MapGet("/", () => "Hello World!");


// ---------------------
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(builder.Environment.ContentRootPath, "CustomFiles")),
//    RequestPath = "/custom"
//});

//app.MapGet("/", () => "Hello World!");


// app.Run();






//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapStaticAssets();

//app.UseStaticFiles(new StaticFileOptions
//{
//    OnPrepareResponse = ctx =>
//    {
//        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
//    }
//});

//app.MapFallbackToFile("index.html");

//app.Run();

