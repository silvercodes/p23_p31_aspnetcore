using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using _04_form.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var env = app.Environment;

var users = new List<User>();
var usersFilePath = Path.Combine(env.ContentRootPath, "Data", "users.json");

// Donload users
if (File.Exists(usersFilePath))
{
    string json = await File.ReadAllTextAsync(usersFilePath);
    users = JsonSerializer.Deserialize<List<User>>(json) ?? users;
}

app.UseStaticFiles();

app.Run(async context =>
{
    var req = context.Request;
    var res = context.Response;
    

    switch(req.Path)
    {
        case "/signup" when req.Method == "GET":
            // send page with form

            try
            {
                var templatePath = Path.Combine(app.Environment.ContentRootPath, "Templates", "register-page.html");
                res.ContentType = "text/html; charset=utf-8;";
                await res.SendFileAsync(templatePath);
            }
            catch (Exception ex)
            {
                await SendError(res, 500, "Registration form unavaliable");
            }
            break;
        case "/signup" when req.Method == "POST":

            try
            {
                if (! req.HasFormContentType)
                {
                    await SendError(res, 415, "Unsupported Media Type");
                    return;
                }

                var form = await req.ReadFormAsync();

                // Validation
                var validationResult = ValidateRegistrationForm(form);
                if (! validationResult.IsValid)
                {
                    await SendError(res, 400, validationResult.ErrorMessage ?? string.Empty);
                    return;
                }

                // Create user
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = form["username"].ToString(),
                    Email = form["email"].ToString(),
                    PasswordHash = HashPassword(form["password"].ToString()),
                    CreatedAt = DateTime.Now,
                };

                // Avatar handling
                var avatarFile = form.Files["avatar"];
                string avatarPath = string.Empty;

                if (avatarFile?.Length > 0)
                {
                    var uploadDir = Path.Combine(env.ContentRootPath, "Storage");
                    var extension = Path.GetExtension(avatarFile.FileName).ToLower();
                    
                    if (! new[] {".jpg", ".jpeg", ".png", ".gif"}.Contains(extension))
                    {
                        await SendError(res, 400, "Invalid image format");
                        return;
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    avatarPath = Path.Combine("Storage", fileName);

                    await using var stream = File.Create(Path.Combine(env.ContentRootPath, avatarPath));

                    await avatarFile.CopyToAsync(stream);
                }

                user.AvatarPath = avatarPath;

                users.Add(user);
                await SaveUsers();

                // Send response
                res.ContentType = "application/json";
                await res.WriteAsJsonAsync(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.AvatarPath,
                    user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                await SendError(res, 500, $"Registration error: {ex.Message}");     // FOR DEBUG!!!
                
            }
            break;
    }
});




app.Run();


async Task SendError(HttpResponse res, int statusCode, string message)
{
    res.StatusCode = statusCode;
    res.ContentType = "application/json; charset=utf-8";
    await res.WriteAsJsonAsync(new { Error = message });
}

(bool IsValid, string? ErrorMessage) ValidateRegistrationForm(IFormCollection frm)
{
    var requiredFields = new[] { "username", "email", "password", "confirm_password" };
    foreach(var field in requiredFields)
    {
        if (!frm.ContainsKey(field))
            return (false, $"Missing required field: {field}");
    }

    if (frm["password"] != frm["confirm_password"])
        return (false, $"Passwords do not match");

    if (! frm["email"].ToString().Contains('@'))
        return (false, $"Invalid email");

    var avatarFile = frm.Files["avatar"];
    if (avatarFile?.Length > 2000000)
        return (false, $"Avatar size exeeds 2Mb limit");

    return (true, null);
}

string HashPassword(string password)
{
    using var sha = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(password);
    var hash = sha.ComputeHash(bytes);

    return Convert.ToBase64String(hash);
}

async Task SaveUsers()
{
    var json = JsonSerializer.Serialize(users);
    await File.WriteAllTextAsync(usersFilePath, json);
}


