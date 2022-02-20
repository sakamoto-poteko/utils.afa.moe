using System.Collections.Concurrent;
using Moe.Afa.Utils.API.Services.SteamApiModels;

namespace Moe.Afa.Utils.API.Services;

public class SteamCacheManager : ISteamCacheManager
{
    private IDictionary<ulong, CacheEntry<StoreGameData>> _storeGameDataCache = new ConcurrentDictionary<ulong, CacheEntry<StoreGameData>>();
    private IDictionary<string, CacheEntry<ulong>> _userIdCache = new ConcurrentDictionary<string, CacheEntry<ulong>>();

    public void SetStoreGameData(ulong appId, StoreGameData storeGameData, uint? expireInMinutes)
    {
        SetCacheEntry(appId, storeGameData, _storeGameDataCache, expireInMinutes);
    }

    public void SetUserId(string nickname, ulong userId, uint? expireInMinutes)
    {
        SetCacheEntry(nickname, userId, _userIdCache, expireInMinutes);
    }

    public StoreGameData GetStoreGameData(ulong appId)
    {
        return GetCacheItem(appId, _storeGameDataCache);
    }

    public ulong GetUserId(string nickname)
    {
        return GetCacheItem(nickname, _userIdCache);
    }

    private void SetCacheEntry<TK, TV>(TK key, TV value, IDictionary<TK, CacheEntry<TV>> cache, uint? expireInMinutes)
    {
        var expiry = expireInMinutes == null ? DateTime.MaxValue : DateTime.UtcNow.AddMinutes(expireInMinutes.Value);

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        cache[key] = new CacheEntry<TV>(value, expiry);
    }

    private TV GetCacheItem<TK, TV>(TK key, IDictionary<TK, CacheEntry<TV>> cache)
    {
        if (cache.TryGetValue(key, out var value))
        {
            return DateTime.UtcNow > value.Expiry ? throw new KeyNotFoundException(): value.Value;
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    private record CacheEntry<TV>(TV Value, DateTime Expiry);
}