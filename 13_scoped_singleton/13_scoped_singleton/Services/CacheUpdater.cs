namespace _13_scoped_singleton.Services;

public class CacheUpdater
{
    private readonly ICacheService cache;
    public CacheUpdater(ICacheService cache)    // Singleton --> ctor
    {
        this.cache = cache;
    }

    public void RefreshCache(IDataProvider dataProvider)    // Scoped --> method
    {
        string freshData = dataProvider.GetCurrentData();
        cache.UpdateCache(freshData);
    }
}
