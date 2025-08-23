using System.IdentityModel.Tokens.Jwt;
using _21_rest_api.Data;
using _21_rest_api.Models;
using _21_rest_api.Models.Auth;
using _21_rest_api.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace _21_rest_api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext db;
    private readonly IConfiguration config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        this.db = db;
        this.config = config;
    }

    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        if (await db.Users.AnyAsync(u => u.Username == request.Username))
            throw new Exception("Username already exists");

        if (await db.Users.AnyAsync(u => u.Email == request.Email))
            throw new Exception("Email already exists");

        Cryptography.CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return GenerateAuthResponse(user);
    }
    public Task<AuthResponse> Login(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    private AuthResponse GenerateAuthResponse(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        { 
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            }),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"],
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = tokenString,
            ExpiresAt = token.ValidTo,
        };
    }
}
