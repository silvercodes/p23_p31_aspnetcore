//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Run(async (context) =>
//{
//    HttpRequest req = context.Request;

//    var method = req.Method;
//    var path = req.Path;
//    var id = req.Query["id"];
//    var userAgent = req.Headers["User-Agent"];

//    Console.WriteLine($"{method} {path} {id} {userAgent}");
//});

//app.Run();


#region HttpResponse
//using System.Text.Json;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

// -------- ������� ������ --------

//app.Run(async (context) =>
//{
//    HttpResponse res = context.Response;

//    res.ContentType = "text/plain";
//    res.StatusCode = 201;

//    await res.WriteAsync("Hello from server");
//    //
//    await res.WriteAsync("\nVasia");
//});

//app.Run();


// ------- ��������� ��������� ���������� � ������-���� -----

//app.Run(async (context) =>
//{
//    HttpResponse res = context.Response;
//    res.StatusCode = 418;
//    res.Headers["X-Api-Version"] = "4.0";

//    await res.WriteAsync("Hello from teapot");
//});

//app.Run();


// ------- �������� html -----

//app.Run(async (context) =>
//{
//    HttpResponse res = context.Response;
//    res.StatusCode = 200;
//    res.ContentType = "text/html; charset=utf-8";

//    await res.WriteAsync($@"
//<!DOCTYPE html>
//<html>
//<head>
//    <title>HTML</title>
//</head>
//<body>
//    <h1>hELLO</h1>
//    <p>Current time: {DateTime.Now.ToShortTimeString()}</p>

//</body>
//</html>
//");
//});

//app.Run();



// ------- ��������������� -----

//app.Run(async (context) =>
//{
//    HttpResponse res = context.Response;

//    res.Redirect("https://google.com", true);

//    await Task.CompletedTask;
//});

//app.Run();



// ------- JSON -----

//app.Run(async (context) =>
//{
//    HttpResponse res = context.Response;

//    res.ContentType = "application/json; charset=utf-8";

//    var data = new { Name = "Petya", Age = 23 };

//    await res.WriteAsync(JsonSerializer.Serialize(data));

//});

//app.Run();





// ------- �������� ����� -----

//app.Run(async (context) =>
//{
//    HttpResponse res = context.Response;

//    res.ContentType = "text/plain";
//    res.Headers.ContentDisposition = "attachment; filename=browserData.txt";

//    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

//    await res.SendFileAsync(filePath);
//});

//app.Run();




// ------- Simple Routing ---- -

//app.Run(async (context) =>
//{
//    HttpRequest req = context.Request;
//    HttpResponse res = context.Response;

//    res.ContentType = "text/plain; charset=utf-8";

//    if (req.Path == "/time")
//        await res.WriteAsync(DateTime.Now.ToLongTimeString());
//    else if (req.Path == "/date")
//        await res.WriteAsync(DateTime.Now.ToShortDateString());
//    else
//    {
//        res.StatusCode = 404;
//        await res.WriteAsync("�������� �� �������");
//    }
//});

//app.Run();




// ------------- ��������� �������� ------------

//app.Run(async context =>
//{
//    var res = context.Response;

//    for (int i = 0; i < 5; ++i)
//    {
//        await res.WriteAsync($"DATA: {i}\n\n");
//        await res.Body.FlushAsync();
//        await Task.Delay(1000);
//    }
//});

//app.Run();



#endregion


#region HttpRequest

using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



//app.Run(async context =>
//{
//    var req = context.Request;
//    var res = context.Response;

//    var report = new 
//    { 
//        Method = req.Method,
//        Path = req.Path,
//        Protocol = req.Protocol,
//        Host = req.Host,
//        QueryString = req.QueryString,
//        ContentType = req.ContentType,
//        ContentLength = req.ContentLength,
//    };

//    await res.WriteAsJsonAsync(report);
//});

//app.Run();





//app.Run(async context =>
//{
//    var req = context.Request;

//    string name = req.Query["name"].ToString();

//    string filter = req.Query.TryGetValue("filter", out var f)
//                    ? f.ToString().ToLower()
//                    : "id";

//    await context.Response.WriteAsJsonAsync(new { name, filter });
//});




//app.Run(async context =>
//{
//    var req = context.Request;
//    var res = context.Response;

//    if (! req.Headers.TryGetValue("X-Api-Key", out var apiKey))
//    {
//        res.StatusCode = StatusCodes.Status401Unauthorized;
//        await res.WriteAsync("API key is required");
//        return;
//    }

//    if (! req.Cookies.TryGetValue("auth-token", out var token))
//    {
//        res.StatusCode = StatusCodes.Status403Forbidden;
//        await res.WriteAsync("Not authenticated");
//        return;
//    }

//    await res.WriteAsync("Access granted");
//    return;
//});





app.Run(async context =>
{
    var req = context.Request;
    var res = context.Response;

    switch(req.Path)
    {
        case "/api/v1/users" when req.Method == "GET":

            break;

        case "/api/v1/users" when req.Method == "POST":

            break;

        default:
            res.StatusCode = StatusCodes.Status404NotFound;
            await res.WriteAsync("Endpoint not found");
            break;
    }
});

app.Run();


#endregion