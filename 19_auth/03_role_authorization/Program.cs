using System.Text;
using _03_role_authorization.Models;
using _03_role_authorization.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserService>();
builder.Services.AddScoped<JwtService>();

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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SupportOnly", policy => policy.RequireRole("Support"));

    options.AddPolicy("AdminOrSupport", policy => policy.RequireRole("Admin", "Support"));

    options.AddPolicy("AdminAndSupport", policy =>
        policy  .RequireRole("Admin")
                .RequireRole("Support"));

});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (
        [FromBody] LoginRequest req,
        UserService userService,
        JwtService jwtService) =>
{
    User? user = userService.ValidateUser(req.Username, req.Password);

    if (user is null)
        return Results.Unauthorized(); // --> 401

    var token = jwtService.GenerateToken(user);

    return Results.Ok(new LoginResponse { Token = token });
}).AllowAnonymous();



app.Run();
