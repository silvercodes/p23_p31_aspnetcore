using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.Run(async ctx =>
//{
//    if (ctx.Request.Path == "/old-page")
//    {
//        ctx.Response.Redirect("/new_page");
//        return;
//    }

//    await ctx.Response.WriteAsync("Main page");
//});


//app.Run(async ctx =>
//{
//    if (ctx.Request.Path == "/old-page")
//    {
//        ctx.Response.Redirect("new-page", true);
//        return;
//    }

//    await ctx.Response.WriteAsync("Main page");
//});


//app.Run(async ctx =>
//{
//    if (ctx.Request.Path == "/old-page")
//    {
//        ctx.Response.Redirect("new-page", true, true);
//        return;
//    }

//    await ctx.Response.WriteAsync("Main page");
//});


//app.Run(async ctx =>
//{
//    if (ctx.Request.Path == "/search")
//    {
//        var query = ctx.Request.Query["question"];
//        ctx.Response.Redirect($"/results?searchTerm={WebUtility.UrlEncode(query)}");
//        return;
//    }

//    await ctx.Response.WriteAsync("Main page");
//});

app.Run();
