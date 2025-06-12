using System.IO.Compression;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var env = app.Environment;


// ===== для больших файлов ========
// 1. Отправка файла через SendFileAsync 
//app.Run(async context =>
//{
//    var req = context.Request;
//    var res = context.Response;

//    if (req.Path == "/download/bigfile")
//    {
//        var filePath = Path.Combine(env.ContentRootPath, "Storage", "patterns.pdf");

//        if (File.Exists(filePath))
//        {
//            res.ContentType = "application/pdf";
//            await res.SendFileAsync(filePath);
//        }
//        else
//        {
//            res.StatusCode = 404;
//        }
//    }
//});


// 2.Отправка файла через потоковую передачу 
//app.Run(async context =>
//{
//    var req = context.Request;
//    var res = context.Response;

//    if (req.Path == "/download/stream")
//    {
//        var filePath = Path.Combine(env.ContentRootPath, "Storage", "patterns.pdf");

//        if (File.Exists(filePath))
//        {
//            await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

//            res.ContentType = "application/pdf";
//            res.Headers.ContentDisposition = $"attachment; filename=\"{Path.GetFileName(filePath)}\"";

//            await fs.CopyToAsync(res.Body);
//        }
//        else
//        {
//            res.StatusCode = 404;
//        }
//    }
//});



// 3. Динамическая генерация файла
//app.Run(async context =>
//{
//    var req = context.Request;
//    var res = context.Response;

//    if (req.Path == "/download/generate")
//    {
//        res.ContentType = "text/plain";
//        res.Headers.ContentDisposition = $"attachment; filename=dynamic.txt";

//        // Генерация
//        for (int i = 0; i < 1000; ++i)
//        {
//            await Task.Delay(10);
//            await res.WriteAsync($"Line: {i}\n");
//            await res.Body.FlushAsync();
//        }
//    }
//});


// 4. Отправка с архивацией на лету
//app.Run(async context =>
//{
//    var req = context.Request;
//    var res = context.Response;

//    if (req.Path == "/download/zip")
//    {
//        res.ContentType = "application/zip";
//        res.Headers.ContentDisposition = $"attachment; filename=archive.zip";

//        using var zipArchive = new ZipArchive(res.BodyWriter.AsStream(), ZipArchiveMode.Create);
//        var entry = zipArchive.CreateEntry("data.txt");

//        await using var writer = new StreamWriter(entry.Open());
//        await writer.WriteLineAsync("Данные с сервера");
//    }
//});



app.Run(async context =>
{
    var req = context.Request;
    var res = context.Response;

    if (req.Path == "/download/multizip")
    {
        res.ContentType = "application/zip";
        res.Headers.ContentDisposition = $"attachment; filename=multi_archive.zip";

        using var zipArchive = new ZipArchive(res.BodyWriter.AsStream(), ZipArchiveMode.Create);
        
        // Text file
        var textEntry = zipArchive.CreateEntry("note.txt");
        await using (var writer = new StreamWriter(textEntry.Open()))
        {
            await writer.WriteLineAsync("Данные с сервера");
            await writer.WriteLineAsync($"Current time: {DateTime.Now.ToShortTimeString()}");
        }

        // .csv file
        var csvEntry = zipArchive.CreateEntry("data.csv");
        await using (var csvStream = csvEntry.Open())
        await using (var csvWriter = new StreamWriter(csvStream))
        {
            await csvWriter.WriteLineAsync("Id,Name,Value");
            for (int i = 1; i <= 5; ++i)
                await csvWriter.WriteLineAsync($"{i},item_{i},{i * 100}");
        }

        // .json file
        var jsonEntry = zipArchive.CreateEntry("logs.json");
        await using (var jsonStream = jsonEntry.Open())
        await using (var jsonWriter = new StreamWriter(jsonStream))
        {
            await jsonWriter.WriteLineAsync(
                JsonSerializer.Serialize(new
                {
                    Date = DateTime.Now.ToLongTimeString(),
                    Value = 123,
                    Comment = "comment"
                })
            );
        }


    }
});




app.Run();
