//WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
//// builder.Services.AddSingleton<IService, Service>

//WebApplication app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    // app.UseSwagger();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();

//app.MapGet("/", () => "<h1>Hello World!</h1>");

//app.Run();


#region Middleware

//using _01_intro;

using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

////app.UseExceptionHandler();
////app.UseHttpsRedirection();
////app.UseStaticFiles();
////app.UseRouting();
////app.UseAuthentication();
////app.UseAuthorization();

////app.UseMiddleware<LoggingMiddleware>();
////app.Use(async (context, next) =>
////{
////    Console.WriteLine("Before");
////    await next();
////    Console.WriteLine("After");
////});

//app.UseWelcomePage();

//app.Run();



app.Use(async (context, next) =>
{
    var sw = Stopwatch.StartNew();
    await next(context);
    sw.Stop();
    Console.WriteLine($"Time = {sw.ElapsedMilliseconds} ms");
});

app.Run(async (context) => 
{
    await Task.Delay(500);
    await context.Response.WriteAsync("VASIA HELLO");
});

app.Run();

#endregion
