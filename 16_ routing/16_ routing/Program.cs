#region Base example
//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//// 1. Middleware до роутинга
//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Before routing");
//    await next();
//});

//// 2. Выбор эндпоинта
//app.UseRouting();

//// 3. Middleware, имеющие доступ к эндпоинту
//app.UseAuthentication();
//app.UseAuthorization();

//// 4. Middleware между выбором и выполнением эндпоинта
//app.Use(async (context, next) =>
//{
//    var ep = context.GetEndpoint();
//    if (ep is not null)
//        Console.WriteLine($"Selected: {ep.DisplayName}");

//    await next();
//});

//// 5. Регистрация эндпоинтов
//#pragma warning disable ASP0014 // Suggest using top level route registrations
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("/", () => "Home page")
//        .WithDisplayName("Home Endpoint");

//    endpoints.MapGet("/secret", () => "Secret Data")
//        .RequireAuthorization()
//        .WithDisplayName("Secret Endpoint");
//});
//#pragma warning restore ASP0014 // Suggest using top level route registrations

//app.Run();
#endregion

#region Map()

// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();

// app.Map("/test", () => "test string");

//app.MapGet("/users", () => "users list");
//app.MapPost("/users", () => "User created");
//app.MapDelete("/users", () => "User deleeted");

// app.MapGet("users/{id}", (int id) => $"User {id}");

//app.Map("/debug", (HttpContext ctx) =>
//{
//    var req = ctx.Request;
//    return $"Method: {req.Method}, Path: {req.Path}";
//});

//app.MapGet("/async", async () =>
//{
//    await Task.Delay(1000);
//    return "DATA";
//});


//var userGroup = app.MapGroup("/users");
//userGroup.MapGet("/", () => "All users");
//userGroup.MapGet("/{id}", (int id) => $"User {id}");
//userGroup.MapPost("/", () => "User created");

// app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Home page");

//app.MapGet("/users/{name}", (string name) => $"Hello {name}");

//app.MapGet("/weather", async () =>
//{
//    await Task.Delay(1000);
//    return new { Temp = 25, Humdity = 60 };
//});

//var api = app.MapGroup("/api");
//api.MapGet("/products", () => new[] { "Laptop", "Phone", "Tablet" });
//api.MapGet("/products/{id}", (int id) => $"Product {id}");

//app.Map("/info", (HttpContext ctx) =>
//{
//    var req = ctx.Request;
//    return $"Method: {req.Method}, Path: {req.Path}";
//});

//app.Run();


#endregion

#region Parameters

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

// 1. Базовые параметры

// app.MapGet("/users/{id}", (int id) => $"User {id}");


// 2. Ограничения типов

//app.MapGet("/posts/{postId:int}", (int postId) => $"Post id {postId}");
//app.MapGet("/products/{title:alpha}", (string title) => $"Title: {title}");


// 3. Необязательные параметры

//app.MapGet("/books/{id?}", (int? id)
//    => id.HasValue ? $"Book {id}" : "All books");

// 4.Значения по - умолчанию
// app.MapGet("/items/{category=all}", (string category) => $"Show {category}");

// 5. Regex
//app.MapGet("/orders/{orderId:regex(^ORD-\\d{{4}}$)}", (string orderId) 
//    => $"Order {orderId}"
//);

// 6. Catch-All
//app.MapGet("/files/{**path}", (string path) => $"Requested: {path}");

// 7. Параметры с дефисом и точкой
//app.MapGet("/products/{*title}", (string title) => $"Product: {title}");

// 8. Multiple parameters
// app.MapGet("/orders/{userId:int}/{orderId:int}", (string userId, string orderId) => $"{userId} -- {orderId}");


// app.Run();


#endregion


#region Binding
// TODO: ????????

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/search", (SearchParams p) => $"Search: {p.Query}, Page: {p.Page}");

//app.Run();
//record SearchParams
//(
//    string Query,
//    int Page = 1,
//    string Sort = "asc"
//);

#endregion


#region Constraints

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

// 1. Data type
//app.MapGet("/int/{id:int}", (int id) => $"Int: {id}");;
//app.MapGet("/bool/{flag:bool}", (bool flag) => $"Bool: {flag}");
//app.MapGet("/guid/{id:guid}", (Guid id) => $"GUID: {id}");

// 2. Length/range
//app.MapGet("/minlen/{text:alpha:minlength(5)}", (string text) => $"TEXT: {text}");
//app.MapGet("/len/{text:length(3,8)}", (string text) => $"TEXT: {text}");

//app.MapGet("/range/{num:int:range(1,100)}", (int num) => $"Num: {num}");


// 3. Files
//app.MapGet("/file/{title:file}", (string title) => $"File: {title}");
//app.MapGet("/nofile/{title:nofile}", (string title) => $"Path: {title}");


//app.Run();

#endregion


#region Cuctom constraint

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<RouteOptions>(options =>
//{
//    options.ConstraintMap.Add("even", typeof(EvenNumberConstraint));
//    options.ConstraintMap.Add("valid-category", typeof(VlaidCaategoryConstraint));
//});

//var app = builder.Build();

//app.MapGet("/even/{num:even}", (int num) => $"NUM: {num}");
//app.MapGet("/store/{category:valid-category}", (string category) => $"Category: {category}");

//app.Run();


//class EvenNumberConstraint : IRouteConstraint
//{
//    public bool Match(
//        HttpContext? httpContext, 
//        IRouter? route, 
//        string routeKey, 
//        RouteValueDictionary values, 
//        RouteDirection routeDirection
//    )
//    {
//        if (!values.TryGetValue(routeKey, out var routeVal))
//            return false;

//        if (!int.TryParse(routeVal?.ToString(), out int number))
//            return false;

//        return number % 2 == 0;
//    }
//}

//class VlaidCaategoryConstraint: IRouteConstraint
//{
//    private readonly string[] validCategories = { "electronics", "books", "products" };
//    public bool Match(
//        HttpContext? httpContext,
//        IRouter? route,
//        string routeKey,
//        RouteValueDictionary values,
//        RouteDirection routeDirection
//    )
//    {
//        if (! values.TryGetValue(routeKey, out var category))
//            return false;

//        return validCategories.Contains(category?.ToString());
//    }
//}

#endregion



#region Complex Example

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    context.Response.ContentType = "text/plain; charset=utf-8;";

//    Console.WriteLine("Middleware1: Начало");
//    await context.Response.WriteAsync("Middleware 1\n");
//    await next();
//    Console.WriteLine("Middleware1: Завершение");
//});

//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Middleware2: Начало");
//    await context.Response.WriteAsync("Middleware 2\n");
//    await next();
//    Console.WriteLine("Middleware2: Завершение");
//});

//app.UseRouting();

//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Middleware3: Начало");

//    Endpoint? endpoint = context.GetEndpoint();
//    if (endpoint is not null)
//        await context.Response.WriteAsync($"Определена конечная точка: {endpoint.DisplayName}\n");
//    else
//        await context.Response.WriteAsync($"Конечная точка не определена\n");

//    await next();

//    Console.WriteLine("Middleware3: Завершение");
//});

//app.MapGet("/", () => "Endpoint: HOME PAGE")
//    .WithDisplayName("Home endpoint");

//app.MapGet("/about", () => "Endpoint: ABOUT US")
//    .WithDisplayName("About endpoint");

//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Middleware4: Начало");
//    await context.Response.WriteAsync("Middleware 4\n");
//    await next();
//    Console.WriteLine("Middleware4: Завершение");
//});

//app.UseEndpoints(eps => { });

//app.Run();


#endregion


#region Query parameters

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

// 1. Автоматическое внедрение
// app.MapGet("/products", (int page, int size) => $"page: {page}, size: {size}");
// app.MapGet("/products", (int page, int? size) => $"page: {page}, size: {size ?? 1}");

// 2. Использование атрибута
// app.MapGet("/users", ([FromQuery(Name = "page_number")] int page) => $"Page: {page}");

// 3. From HttpContext
//app.MapGet("/orders", (HttpContext context) =>
//{
//    var sort = context.Request.Query["sort"].FirstOrDefault() ?? "id";
//    var desc = bool.Parse(context.Request.Query["desc"].FirstOrDefault() ?? "false");

//    return $"sort: {sort}, desc: {desc}";
//});

// 4. Привязка к объекту
//app.MapGet("/products", ([AsParameters] FilterOptions filter) => filter);

//app.Run();

//public record FilterOptions
//(
//    string Category,
//    double? MinPrice = null,
//    double? MaxPrice = null,
//    string SortBy = "name"
//);


// 5. Arrays and lists
//app.MapGet("/tags", (string[] tags) => $"Tags: {string.Join(',', tags)}");

// app.Run();

#endregion
