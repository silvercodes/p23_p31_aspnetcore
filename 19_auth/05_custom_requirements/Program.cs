using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using _05_custom_requirements.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

builder.Services.AddSingleton<IAuthorizationHandler, BusinessHoursHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BusinessHoursOnly", policy =>
        policy.Requirements.Add(new BusinessHoursRequirement(
            startTime: new TimeOnly(9, 0),
            endTime: new TimeOnly(18, 0))));

    options.AddPolicy("AdminAnyTime", policy =>
    {
        policy.RequireRole("admin");
        policy.Requirements.Add(new BusinessHoursRequirement(
            startTime: new TimeOnly(0, 0),
            endTime: new TimeOnly(23, 59, 59)));
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/generate-token/{role}", (string role) =>
{
    var claims = new List<Claim>
    {
        new(ClaimTypes.Name, $"user-{Guid.NewGuid()}"),
        new(ClaimTypes.Role, role)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes("poewrlkahfdsoiuqwerhl97234khs8lskdjfaoierlksflkjo"));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "TestIssuer",
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);

});

app.MapGet("/business-resources", () => "Business resources available")
    .RequireAuthorization("BusinessHoursOnly");

app.MapGet("/admin-resources", () => "Admin resources")
    .RequireAuthorization("AdminAnyTime");

app.MapGet("/time", () => $"Current server time: {DateTime.Now:HH:mm:ss}");

app.MapGet("/access-info", (HttpContext ctx, IAuthorizationService authService) =>
{
    var requirement = new BusinessHoursRequirement(new TimeOnly(9, 0), new TimeOnly(18, 0));

    var result = authService.AuthorizeAsync(ctx.User, null, "BusinessHoursOnly").Result;

    return new
    {
        CurrentTime = DateTime.Now.ToString("HH:mm:ss"),
        IsAuthenticated = ctx.User.Identity?.IsAuthenticated,
        UserName = ctx.User.Identity?.Name,
        Roles = ctx.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value),
        HasBusinessHoursAccess = result.Succeeded  
    };
});



app.Run();
