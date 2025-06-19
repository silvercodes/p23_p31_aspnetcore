
#region Use()
//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();


//app.Use(async (context, next) =>
//{
//    // Логика до передачи в next

//    //await next.Invoke(context);       // next --> RequestDelegate
//    // or
//    //await next();                     // next --> Func<Task>          :-)

//    // Логика после возврата из next
//});

//app.Run();





//string time = string.Empty;

//app.Use(async (context, next) =>
//{
//    // Логика до передачи в next
//    time = DateTime.Now.ToShortTimeString();

//    await next();

//    // Логика после возврата из next
//    Console.WriteLine($"time: {time}");
//});

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync($"time: { time}");
//});

//app.Run();





//app.Use(async (context, next) =>
//{
//    // До next можно писать в ответ
//    context.Response.ContentType = "text/plain; charset=utf-8";
//    await context.Response.WriteAsync("До вызова next()\n");

//    await next();

//    // После next ОПАСНО писать в ответ
//    await context.Response.WriteAsync("После вызова next()");

//});

//app.Run();



// ==============================
//app.Use(async (ctx, next) =>
//{
//    string? path = ctx.Request.Path.Value;
//    if (path == "/time")
//        await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}");
//    else
//        await next();
//});

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync($"Terminal MW");
//});

//app.Run();

// ----- Отдельный метод с Func<Task>

//async Task GetTime(HttpContext ctx, Func<Task> next)
//{
//    string? path = ctx.Request.Path.Value;
//    if (path == "/time")
//        await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}");
//    else
//        await next();
//}

//app.Use(GetTime);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync($"Terminal MW");
//});

//app.Run();



// ----- Отдельный метод с RequestDelegate

//async Task GetTime(HttpContext ctx, RequestDelegate next)
//{
//    string? path = ctx.Request.Path.Value;
//    if (path == "/time")
//        await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}");
//    else
//        await next.Invoke(ctx);
//}

//app.Use(GetTime);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync($"Terminal MW");
//});

//app.Run();
#endregion


#region UseWhen() / MapWhen()

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseWhen(
//    ctx => ctx.Request.Path == "/time",
//    app =>
//    {
//        app.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"time: {time}");
//            await next();
//        });

//        app.Run(async context =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await context.Response.WriteAsync($"time: {time}");
//        });
//    }
//);

//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"FROM terminal MW");
//});

//app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseWhen(
//    ctx => ctx.Request.Path == "/time",
//    app =>
//    {
//        app.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"time: {time}");
//            await next();
//        });

//        app.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await context.Response.WriteAsync($"time: {time}");
//            await next();
//        });
//    }
//);

//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"FROM terminal MW");
//});

//app.Run();






//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseWhen(
//    ctx => ctx.Request.Path == "/time",
//    BuildBranch
//);

//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"FROM terminal MW");
//});

//app.Run();

//void BuildBranch(IApplicationBuilder app)
//{
//    app.Use(async (context, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        Console.WriteLine($"time: {time}");
//        await next();
//    });

//    app.Use(async (context, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        await context.Response.WriteAsync($"time: {time}");
//        await next();
//    });
//}




//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapWhen(
//    ctx => ctx.Request.Path == "/time",
//    app =>
//    {
//        app.Run(async context =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await context.Response.WriteAsync($"time: {time}");
//        });
//    }
//);

//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"FROM terminal MW");
//});

//app.Run();

#endregion


#region Map()

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Map("/time", app =>
//{
//    var time = DateTime.Now.ToShortTimeString();
//    app.Use(async (ctx, next) =>
//    {
//        Console.WriteLine(time);
//        await next();
//    });

//    app.Run(async ctx => await ctx.Response.WriteAsync(time));
//});

//app.Run(async ctx => await ctx.Response.WriteAsync("END"));

//app.Run();



//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Map("/time", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}")));
//app.Map("/date", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortDateString()}")));
//app.Map("/home", app => app.Run(async ctx => await ctx.Response.WriteAsync($"HOME")));

//app.Run(async ctx =>
//{
//    ctx.Response.StatusCode = 404;
//    await ctx.Response.WriteAsync("Endpoint not found :-(");
//});

//app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Map("/api", app =>
//{
//    app.Map("/time", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}")));
//    app.Map("/date", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortDateString()}")));
//    app.Map("/home", app => app.Run(async ctx => await ctx.Response.WriteAsync($"HOME")));
//    app.Run(async ctx =>
//    {
//        ctx.Response.StatusCode = 404;
//        await ctx.Response.WriteAsync("Branch endpoint not found :-(");
//    });
//});

//app.Run(async ctx =>
//{
//    ctx.Response.StatusCode = 404;
//    await ctx.Response.WriteAsync("Endpoint not found :-(");
//});

//app.Run();





// ------------- Пример с версиями API

// /login
// home
// /api/v1/users
// /api/v2/users

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//// Use...
//// Use...

//app.Map("/api", apiApp =>
//{
//    apiApp.Use(async (context, next) =>
//    {
//        await context.Response.WriteAsync("API Logger\n");
//        await next();
//    });

//    apiApp.Map("/v1", v1App => 
//    {
//        v1App.Map("/users", v1AppUsers =>
//        {
//            v1AppUsers.Run(async context =>
//            {
//                await context.Response.WriteAsync("API V1 users\n");
//            });
//        });

//        v1App.Run(async context =>
//        {
//            context.Response.StatusCode = 404;
//            await context.Response.WriteAsync("API V1 endpoint not found\n");
//        });
//    });

//    apiApp.Map("/v2", v2App =>
//    {
//        v2App.Map("/users", v2AppUsers =>
//        {
//            v2AppUsers.Run(async context =>
//            {
//                await context.Response.WriteAsync("API V2 users\n");
//            });
//        });

//        v2App.Run(async context =>
//        {
//            context.Response.StatusCode = 404;
//            await context.Response.WriteAsync("API V2 endpoint not found\n");
//        });
//    });

//});

//app.Run();





// ------------- Пример со статическими файлами

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//var env = app.Environment;

//app.Map("/static", staticApp =>
//{
//    staticApp.Run(async ctx =>
//    {
//        var filePath = Path.Combine(env.ContentRootPath, "static", Path.GetFileName(ctx.Request.Path));
//        ctx.Response.ContentType = "application/octet-stream";
//        await ctx.Response.SendFileAsync(filePath);

//    });
//});

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Main");
//});

//app.Run();





// ------------- Пример с аутентификацией

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//var env = app.Environment;

//// "/api/public...."
//// "/api/secure..."

//app.Map("/api/public", publicApp =>
//{
//    publicApp.Run(async (ctx) =>
//    {
//        await ctx.Response.WriteAsync("Public content");
//    });
//});

//app.Map("/api/secure", secureApp =>
//{
//    secureApp.Use(async (ctx, next) =>
//    {
//        if (ctx.Request.Headers["X-auth-token"] != "secret123")
//        {
//            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
//            await ctx.Response.WriteAsync("User access denied!");
//            return;
//        }

//        await next();
//    });

//    secureApp.Run(async (ctx) =>
//    {
//        await ctx.Response.WriteAsync("Secure content");
//    });
//});

//app.Run();


#endregion


#region Middleware classes

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseMiddleware<CustomMiddleware>();
//app.Run(async ctx => await ctx.Response.WriteAsync("Main"));
//app.Run();

//public class CustomMiddleware
//{
//    private readonly RequestDelegate next;

//    public CustomMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        Console.WriteLine("one");

//        await next(context);

//        Console.WriteLine("two");
//    }
//}









//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();


//app.UseCustomMiddleware();
//app.Run(async ctx => await ctx.Response.WriteAsync("Main"));
//app.Run();

//public class CustomMiddleware
//{
//    private readonly RequestDelegate next;

//    public CustomMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        Console.WriteLine("one");

//        await next(context);

//        Console.WriteLine("two");
//    }
//}

//public static class CustomAppExtensions
//{
//    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
//    {
//        return app.UseMiddleware<CustomMiddleware>();
//    }
//}







//using System.Diagnostics;


//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();


//app.UseMiddleware<TimingMiddleware>(true);
//app.Run(async ctx => await ctx.Response.WriteAsync("Main"));
//app.Run();

//class TimingMiddleware
//{
//    private readonly RequestDelegate next;
//    private readonly bool logToConsole;

//    public TimingMiddleware(RequestDelegate next, bool logToConsole = false)
//    {
//        this.next = next;
//        this.logToConsole = logToConsole;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        var sw = Stopwatch.StartNew();
//        await next(context);
//        sw.Stop();

//        if (logToConsole)
//            Console.WriteLine($"Request to {context.Request.Path} took {sw.ElapsedMilliseconds} ms");
//    }
//}





//--------------- Поддомены

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();


//app.UseWhen(
//    context => context.Request.Host.Host.StartsWith("admin."),
//    branch => branch.UseMiddleware<AdminDashboardMiddleware>()
//);

//app.UseWhen(
//    context => context.Request.Host.Host.StartsWith("api."),
//    branch => branch.UseMiddleware<ApiVersioningMiddleware>()
//);

//app.MapGet("/", async context =>
//{
//    string data = $"Dashboard: {context.Items["DashboardType"]}, Api version: {context.Request.Headers["X-Api-Version"]}";
//    await context.Response.WriteAsync(data);
//});

//app.Run();


//public class AdminDashboardMiddleware
//{
//    private readonly RequestDelegate next;
//    public AdminDashboardMiddleware(RequestDelegate next) => this.next = next;
//    public async Task InvokeAsync(HttpContext context)
//    {
//        context.Items["DashboardType"] = "Admin";

//        await next(context);
//    }
//}

//public class ApiVersioningMiddleware
//{
//    private readonly RequestDelegate next;
//    public ApiVersioningMiddleware(RequestDelegate next) => this.next = next;
//    public async Task InvokeAsync(HttpContext context)
//    {
//        context.Request.Headers["X-Api-Version"] = "v2";

//        await next(context);
//    }
//}




// ---------------  Разная аутентификация для api/ и admin/


//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseWhen(
//    context => context.Request.Path.StartsWithSegments("/admin"),
//    branch => branch.UseMiddleware<AdminAuthMiddleware>()
//);

//app.UseWhen(
//    context => context.Request.Path.StartsWithSegments("/api"),
//    branch => branch.UseMiddleware<ApiAuthMiddleware>()
//);

//app.MapGet("/admin/dashboard", () => "Admin dashboard");
//app.MapGet("/api/users", () => "Users list");
//app.MapGet("/", () => "Home page");

//app.Run();

//class AdminAuthMiddleware
//{
//    private readonly RequestDelegate next;
//    public AdminAuthMiddleware(RequestDelegate next) => this.next = next;
//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Headers["X-Admin-Token"] != "123123123")
//        {
//            context.Response.StatusCode = 401;
//            await context.Response.WriteAsync("Admin access denied");
//            return;
//        }

//        await next(context);
//    }
//}

//class ApiAuthMiddleware
//{
//    private readonly RequestDelegate next;
//    public ApiAuthMiddleware(RequestDelegate next) => this.next = next;
//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Query["api_key"] != "key123")
//        {
//            context.Response.StatusCode = 401;
//            await context.Response.WriteAsync("Invlaid API key");
//            return;
//        }

//        await next(context);
//    }
//}

#endregion



