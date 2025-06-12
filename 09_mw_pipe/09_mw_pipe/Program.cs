
#region Use()
//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();


//app.Use(async (context, next) =>
//{
//    // ������ �� �������� � next

//    //await next.Invoke(context);       // next --> RequestDelegate
//    // or
//    //await next();                     // next --> Func<Task>          :-)

//    // ������ ����� �������� �� next
//});

//app.Run();





//string time = string.Empty;

//app.Use(async (context, next) =>
//{
//    // ������ �� �������� � next
//    time = DateTime.Now.ToShortTimeString();

//    await next();

//    // ������ ����� �������� �� next
//    Console.WriteLine($"time: {time}");
//});

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync($"time: { time}");
//});

//app.Run();





//app.Use(async (context, next) =>
//{
//    // �� next ����� ������ � �����
//    context.Response.ContentType = "text/plain; charset=utf-8";
//    await context.Response.WriteAsync("�� ������ next()\n");

//    await next();

//    // ����� next ������ ������ � �����
//    await context.Response.WriteAsync("����� ������ next()");

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

// ----- ��������� ����� � Func<Task>

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



// ----- ��������� ����� � RequestDelegate

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





var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/api", app =>
{
    app.Map("/time", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}")));
    app.Map("/date", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortDateString()}")));
    app.Map("/home", app => app.Run(async ctx => await ctx.Response.WriteAsync($"HOME")));
});

app.Run(async ctx =>
{
    ctx.Response.StatusCode = 404;
    await ctx.Response.WriteAsync("Endpoint not found :-(");
});

app.Run();

#endregion
