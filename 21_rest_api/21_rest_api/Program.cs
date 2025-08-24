

using _21_rest_api.Data;
using _21_rest_api.Endpoints;
using _21_rest_api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // db.Database.Migrate();
    DbInitializer.Initialize(db);
}
// ---------------------------
app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapAuthEndpoints();


app.Run();
