using _21_rest_api.Models;
using _21_rest_api.Tools;
using Microsoft.EntityFrameworkCore;

namespace _21_rest_api.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();

        if (context.Users.Any())
            return;

        var admin = new User
        {
            Username = "admin",
            Email = "admin@mail.com",
            Role = "Admin"
        };

        // Password admin123
        Cryptography.CreatePasswordHash("admin123", out byte[] hash, out byte[] salt);
        admin.PasswordHash = hash;
        admin.PasswordSalt = salt;

        context.Users.Add(admin);
        context.SaveChanges();
    }
}
