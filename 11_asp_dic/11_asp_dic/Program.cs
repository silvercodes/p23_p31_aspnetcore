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
//    string res = rnd.NextValue(0, 2) == 0 ? "Орёл" : "Решка";
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

#region AddHttpClient()

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHttpClient();
//builder.Services.AddSingleton<DataService>();

//var app = builder.Build();

//app.MapGet("/", async (HttpContext ctx, DataService service) =>
//{
//    return await service.GetData();
//});

//app.Run();

//class DataService
//{
//    private readonly HttpClient httpClient;

//    public DataService(HttpClient httpClient)
//    {
//        this.httpClient = httpClient;
//    }

//    public async Task<string> GetData()
//    {
//        var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
//        return await response.Content.ReadAsStringAsync();
//    }
//}




//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHttpClient("jsonph", client =>
//{
//    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
//    client.DefaultRequestHeaders.Add("User-Agent", "MyApp");
//    client.Timeout = TimeSpan.FromSeconds(15);
//});
//builder.Services.AddSingleton<DataService>();

//var app = builder.Build();

//app.MapGet("/", async (HttpContext ctx, DataService service) =>
//{
//    return await service.GetData();
//});

//app.Run();

//class DataService
//{
//    private readonly IHttpClientFactory cFactory;

//    public DataService(IHttpClientFactory cFactory)
//    {
//        this.cFactory = cFactory;
//    }

//    public async Task<string> GetData()
//    {
//        var client = cFactory.CreateClient("jsonph");
//        var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
//        return await response.Content.ReadAsStringAsync();
//    }
//}



//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<DataService>();

//builder.Services.AddHttpClient<DataService>(client =>
//{
//    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//    client.Timeout = TimeSpan.FromSeconds(15);
//});


//var app = builder.Build();

//app.MapGet("/", async (HttpContext ctx, DataService service) =>
//{
//    return await service.GetData();
//});

//app.Run();

//class DataService
//{
//    private readonly HttpClient httpClient;

//    public DataService(HttpClient httpClient)
//    {
//        this.httpClient = httpClient;
//    }

//    public async Task<string> GetData()
//    {
//        return await httpClient.GetStringAsync("users");
//    }
//}


#endregion

#region Example_2

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
//{
//    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
//});

//var app = builder.Build();


//app.MapGet("/users", async (HttpContext ctx, IUserService userService) =>
//{
//    await ctx.Response.WriteAsJsonAsync(await userService.GetUsersAsync());

//    // return await userService.GetUsersAsync();
//});

//app.Run();


//// ------
//public interface IUserService
//{
//    Task<User[]?> GetUsersAsync();
//}
//public class UserService(IUserApiClient api) : IUserService
//{
//    public async Task<User[]?> GetUsersAsync() =>
//        await api.GetUsersAsync();
//}
//// ------
//public interface IUserApiClient
//{
//    Task<User[]?> GetUsersAsync();
//}
//public class UserApiClient(HttpClient http) : IUserApiClient
//{
//    public async Task<User[]?> GetUsersAsync() =>
//        await http.GetFromJsonAsync<User[]>("/users");
//}

//public record class User(int Id, string Name, string Username, string Email);

#endregion

#region Get services
// --- Use ctor

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IRandomService, RandomService>();
//builder.Services.AddSingleton<MyService>();

//var app = builder.Build();

//app.Run();

//public class MyService
//{
//    private readonly IRandomService randomService;

//    public MyService(IRandomService randomService)
//    {
//        this.randomService = randomService;
//    }
//}



// --- Use parameters

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IRandomService, RandomService>();
//builder.Services.AddSingleton<MyService>();

//var app = builder.Build();

//app.MapGet("/", async (HttpContext ctx, MyService service) =>
//{
//    await ctx.Response.WriteAsync(service.GetVal().ToString());
//});

//app.Run();

//public class MyService
//{
//    private readonly IRandomService randomService;

//    public MyService(IRandomService randomService)
//    {
//        this.randomService = randomService;
//    }
//    public int GetVal() => randomService.NextValue(1, 100);
//}


// ---- Ручное получение из HttpContext
//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IRandomService, RandomService>();
//var app = builder.Build();

//app.MapGet("/manual", async (HttpContext ctx) =>
//{
//    var rnd = ctx.RequestServices.GetRequiredService<IRandomService>();
//    await ctx.Response.WriteAsync(rnd.NextValue(1, 100).ToString());
//});

//app.Run();


// --- Получение после app.Build()
//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IRandomService, RandomService>();
//var app = builder.Build();

//using(var scope = app.Services.CreateScope())
//{
//    var scopedService = scope.ServiceProvider.GetRequiredService<IRandomService>();
//    Console.WriteLine(scopedService.NextValue(1, 100));
//}

//app.Run();




//public interface IRandomService
//{
//    int NextValue(int min, int max);
//}
//public class RandomService : IRandomService
//{
//    private readonly Random random = new Random();
//    public int NextValue(int min, int max)
//    {
//        return random.Next(min, max);
//    }
//}

#endregion

#region service LifeCycle

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<SingletonService>();
builder.Services.AddScoped<ScopedService>();
builder.Services.AddTransient<TransientService>();

var app = builder.Build();

app.MapGet("/", (
    SingletonService singletonService_1,
    SingletonService singletonService_2,
    ScopedService scopedService_1,
    ScopedService scopedService_2,
    TransientService transientService_1,
    TransientService transientService_2
) => {
    return new
    {
        Singleton = new
        {
            Id1 = singletonService_1.Id,
            Id2 = singletonService_2.Id,
            Created = singletonService_1.Created,
            Equal = singletonService_1.Id == singletonService_2.Id
        },
        Scoped = new
        {
            Id1 = scopedService_1.Id,
            Id2 = scopedService_2.Id,
            Created = scopedService_1.Created,
            Equal = scopedService_1.Id == scopedService_2.Id
        },
        Transient = new
        {
            Id1 = transientService_1.Id,
            Id2 = transientService_2.Id,
            Created1 = transientService_1.Created,
            Created2 = transientService_2.Created,
            Equal = transientService_1.Id == transientService_2.Id
        },

    };
});

app.Run();

public class SingletonService
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Created { get; } = DateTime.Now.ToString("HH:mm:ss.fff");
}
public class ScopedService
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Created { get; } = DateTime.Now.ToString("HH:mm:ss.fff");
}
public class TransientService
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Created { get; } = DateTime.Now.ToString("HH:mm:ss.fff");
}

#endregion
