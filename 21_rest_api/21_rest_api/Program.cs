

using _21_rest_api.Data;
using _21_rest_api.Endpoints;
using _21_rest_api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Task Items API",
        Version = "v1",
        Description = "Тестовый API для управления задачами",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Vladimir",
            Email = "silver.itstep@gmail.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization header using Bearer scheme...",
        Name = "Authorization",     // header name,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
       
    });
});

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();
});
builder.Services.AddApplicationServices();
builder.Services.ConfigureCors();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // db.Database.Migrate();
    DbInitializer.Initialize(db);
}
// ---------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Items v1");
    });
}

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapAuthEndpoints();
app.MapTaskItemEndpoints();


app.Run();
