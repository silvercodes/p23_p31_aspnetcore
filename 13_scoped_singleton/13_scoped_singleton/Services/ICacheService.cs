namespace _13_scoped_singleton.Services;

public interface ICacheService
{
    string GetCachData();
    void UpdateCache(string newData);
}
