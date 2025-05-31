
using System.Text;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "public",
});

var app = builder.Build();

app.UseStaticFiles();

app.Use(async (context, next) =>
{
    HttpRequest req = context.Request;

    if (req.Path == "/" && req.Method == "GET")
    {
        var templatePath = Path.Combine(app.Environment.ContentRootPath, "Templates", "index.html");
        var htmlContent = await File.ReadAllTextAsync(templatePath);

        var imageDirectory = Path.Combine(app.Environment.WebRootPath, "store");
        var imageFiles = Directory.GetFiles(imageDirectory);

        var imgHtml = new StringBuilder();
        foreach (var item in imageFiles)
        {
            var fileName = Path.GetFileName(item);
            imgHtml.AppendLine(
                $@" <a href=""/download/{fileName}"">
                        <img src=""/store/{fileName}"" width=""200"" />
                    </a>"
            );
        }

        var finalHtml = htmlContent.Replace("<!--IMAGES-->", imgHtml.ToString());
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(finalHtml);
    }
    else
    {
        await next(context);
    }
});

app.Use(async (context, next) =>
{
    HttpRequest req = context.Request;
    HttpResponse res = context.Response;

    if (req.Method == "GET" && req.Path.StartsWithSegments("/download", out var remaining))
    {
        var imageName = Path.GetFileName(remaining.ToString());
        
        if (string.IsNullOrEmpty(imageName))
        {
            res.StatusCode = StatusCodes.Status400BadRequest;
            await res.WriteAsync("Invalid image name");
            return;
        }

        var imageDirectory = Path.Combine(app.Environment.WebRootPath, "store");
        var imagePath = Path.Combine(imageDirectory, imageName);

        if (!File.Exists(imagePath))
        {
            res.StatusCode = StatusCodes.Status404NotFound;
            await res.WriteAsync("File not found");
            return;
        }

        // --- download image
        //res.ContentType = "application/octet-stream";
        //res.Headers["Content-Disposition"] = $"attachment; filename={imageName}";

        // --- open image
        res.ContentType = "image/jpg";

        await using (var fileStream = File.OpenRead(imagePath))
        {
            await fileStream.CopyToAsync(res.Body);
        }
    }
    else
    {
        await next(context);
    }
});

app.Run();
