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
        policy.RequireRole("Admin");
        policy.Requirements.Add(new BusinessHoursRequirement(
            startTime: new TimeOnly(0, 0),
            endTime: new TimeOnly(23, 59)));
    });
});


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
