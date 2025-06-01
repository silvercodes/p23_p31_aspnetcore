var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async context =>
{
    var req = context.Request;
    var res = context.Response;

    switch(req.Path)
    {
        case "/signup" when req.Method == "GET":
            // send page with form

            try
            {
                var templatePath = Path.Combine(app.Environment.ContentRootPath, "Templates", "register-page.html");
                res.ContentType = "text/html; charset=utf-8;";
                await res.SendFileAsync(templatePath);
            }
            catch (Exception ex)
            {
                await SendError(res, 500, "Registration form unavaliable");
            }
            break;
        case "/signup" when req.Method == "POST":
            // register user
            break;
    }
});




app.Run();


async Task SendError(HttpResponse res, int statusCode, string message)
{
    res.StatusCode = statusCode;
    res.ContentType = "application/json; charset=utf-8";
    await res.WriteAsJsonAsync(new { Error = message });
}