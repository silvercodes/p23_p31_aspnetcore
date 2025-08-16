using _02_jwt_auth.Models;
using _02_jwt_auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace _02_jwt_auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/login", (
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

        authGroup.MapGet("/protected", () => "This is protected endpoint (JWT token only)")
            .RequireAuthorization();

        authGroup.MapGet("/admin", () => "This ISDATE admin only data")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}
