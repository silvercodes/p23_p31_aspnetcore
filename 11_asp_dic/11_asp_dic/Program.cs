#region Embedded services list
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//var services = builder.Services;

//var app = builder.Build();

//app.MapGet("/", async ctx =>
//{
//    int count = 0;
//    var builder = new StringBuilder();
//    builder.Append("<table>");
//    builder.Append("<tr><th>#</th><th>Abstraction</th><th>Implementation</th></tr>");

//    foreach (var item in services)
//    {
//        builder.Append("<tr>");
//        builder.Append($"<td>{++count}</td>");
//        builder.Append($"<td>{item.ServiceType.Name}</td>");
//        builder.Append($"<td>{item.ImplementationType?.Name}</td>");
//        builder.Append("</tr>");
//    }

//    builder.Append("</table>");

//    ctx.Response.ContentType = "text/html";
//    await ctx.Response.WriteAsync(builder.ToString());
//});

//app.Run();

#endregion


#region Example_1
//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddScoped<IRandomService, RandomService>();

//var app = builder.Build();

//app.Use(async (ctx, next) =>
//{
//    ctx.Response.ContentType = "text/plain;charset=utf-8";

//    await next();
//});

//app.MapGet("/random", async (HttpContext ctx, IRandomService rnd) =>
//{
//    await ctx.Response.WriteAsync($"count: {rnd.Count} random: {rnd.NextValue(1, 100)}");
//});

//app.MapGet("/day", async (HttpContext ctx, IRandomService rnd) =>
//{
//    await ctx.Response.WriteAsync($"count: {rnd.Count} day: {rnd.NextValue(1, 8)}");
//});

//app.MapGet("/coin", async (HttpContext ctx, IRandomService rnd) =>
//{
//    string res = rnd.NextValue(0, 2) == 0 ? "Îð¸ë" : "Ðåøêà";
//    await ctx.Response.WriteAsync($"count: {rnd.Count} coin: {res}");
//});


//app.Run();


//public interface IRandomService
//{
//    public int Count { get; set; }
//    int NextValue(int min, int max);
//}
//public class RandomService : IRandomService
//{
//    public int Count { get; set; } = 0;
//    private readonly Random random = new Random();
//    public int NextValue(int min, int max)
//    {
//        ++Count;
//        return random.Next(min, max);
//    }
//}

#endregion


#region Example_2

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpClient(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

var app = builder.Build();

app.MapGet("/users", async (HttpContext ctx, IUserService userService) =>
{
    await ctx.Response.WriteAsJsonAsync(await userService.GetUsersAsync());

    // return await userService.GetUsersAsync();
});

app.Run();



public interface IUserService
{
    Task<User[]> GetUsersAsync();
}
public class UserService : IUserService
{
    public Task<User[]> GetUsersAsync()
    {
        throw new NotImplementedException();
    }
}




public record class User(int Id, string Name, string Username, string Email);

#endregion



