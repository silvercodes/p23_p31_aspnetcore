using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

const string loginForm = """
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Login</title>
</head>
<body>
    <form action="/login" method="post">
        <input type="text" name="username" placeholder="Username" required>
        <input type="password" name="password" placeholder="Password" required>
        <button type="submit">LOGIN</button>
    </form>
</body>
</html>
""";


var builder = WebApplication.CreateBuilder(args);

// Создать схему с хендлером(-ами)
// добавить сервис аутентификации
builder.Services.AddAuthentication("MyCookie")
    .AddCookie("MyCookie", options => 
    {
        options.Cookie.Name = "AuthCookie";
        options.LoginPath = "/login";
    });

// Добавить сервис авторизации
builder.Services.AddAuthorization();

var app = builder.Build();

// Включить middlewares в конвейер (ПОРЯДОК ВАЖЕН)
app.UseAuthentication();
app.UseAuthorization();

// Эндпоинты
app.MapGet("/", (HttpContext ctx) =>
{
    return ctx.User.Identity?.IsAuthenticated == true
        ? $"Hello {ctx.User.Identity.Name}!"
        : "Hello noname!";
});

app.MapGet("/login", () => Results.Content(loginForm, "text/html"));

app.MapPost("/login", async (HttpContext ctx) =>
{
    const string validUsername = "vasia";
    const string validPasseord = "qwerty123";

    var req = ctx.Request;

    if (req.Form["username"] == validUsername && req.Form["password"] == validPasseord)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, "vasia"),
            new (ClaimTypes.Email, "vasia@mail.com"),
            new (ClaimTypes.Role, "User")
        };

        var identity = new ClaimsIdentity(claims, "MyCookie");
        var principal = new ClaimsPrincipal(identity);

        await ctx.SignInAsync("MyCookie", principal);

        return Results.Redirect("/");
    }

    return Results.Unauthorized();

});






app.Run();
