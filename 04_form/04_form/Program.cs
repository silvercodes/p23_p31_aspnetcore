var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.Run(async context =>
{
    var req = context.Request;
    var res = context.Response;
    var env = app.Environment;

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

                // Avatar handling
                var avatarFile = form.Files["avatar"];

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
                    var avatarPath = Path.Combine("Storage", fileName);

                    await using var stream = File.Create(Path.Combine(env.ContentRootPath, avatarPath));

                    await avatarFile.CopyToAsync(stream);
                }









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