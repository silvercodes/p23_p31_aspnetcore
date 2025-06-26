using _13_scoped_singleton.Services;

namespace _13_scoped_singleton.Middleware;

public class CacheMiddleware
{
    private readonly RequestDelegate next;

    public CacheMiddleware(RequestDelegate next) => this.next = next;

    public async Task InvokeAsync(
        HttpContext context,
        CacheUpdater updater,
        IDataProvider provider
    )
    {
        if (context.Request.Query.ContainsKey("refresh"))
        {
            updater.RefreshCache(provider);
        }

        await next(context);
    }
}
