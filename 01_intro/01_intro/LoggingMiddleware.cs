namespace _01_intro;

public class LoggingMiddleware
{
    private readonly RequestDelegate next;
    public LoggingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Request {context.Request.Path}");
        await next(context);
        Console.WriteLine($"Response {context.Response.StatusCode}");
    }
}
