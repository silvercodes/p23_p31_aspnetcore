using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "TestIssuer",
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("poewrlkahfdsoiuqwerhl97234khs8lskdjfaoierlksflkjo"))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ITOnly", policy =>
        policy.RequireClaim("Department", "IT"));

    options.AddPolicy("TopSecret", policy =>
        policy.RequireClaim("TopLevel", "TopSecret"));

    options.AddPolicy("FinanceManager", policy =>
    {
        policy.RequireClaim("Department", "Finance");
        policy.RequireClaim("Position", "Manager");
    });

    options.AddPolicy("CanEditProfile", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            // ID пользователя, который аутентифицировался
            var userId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return false;

            // ID пользователя/профиля, который хотят редактировать (как пример берем из ресурсов)
            if (ctx.Resource is not HttpContext httpCtx)
                return false;

            if (!httpCtx.Request.RouteValues.TryGetValue("userId", out var routeUserId))
                return false;

            var requestedUserId = routeUserId?.ToString();

            return userId == requestedUserId ||
                ctx.User.IsInRole("admin");
        });
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/generate-token/{role}", (string role) =>
{
    var claims = new List<Claim>
    { 
        new(ClaimTypes.Name, "user123"),
        new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        new("Department", role == "admin" ? "IT": "Finance"),
        new("Position", role == "admin" ? "Director" : "Analyst"),
        new("TopLevel", role == "admin" ? "TopSecret" : "Common"),
        new(ClaimTypes.Role, role)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes("poewrlkahfdsoiuqwerhl97234khs8lskdjfaoierlksflkjo"));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "TestIssuer",
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
});

app.MapGet("/it-resource", () => "IT Department resources")
    .RequireAuthorization("ITOnly");

app.MapGet("/top-secret", () => "TOP SECRET")
    .RequireAuthorization("TopSecret");

app.MapGet("/profile/{userId}", (string userId) => $"Profile is {userId}")
    .RequireAuthorization("CanEditProfile");

app.Run();
