using Moe.Afa.Utils.API.Services.Models.Steam;

namespace Moe.Afa.Utils.API.Services;

public interface ISteamCacheManager
{
    public void SetStoreGameData(ulong appId, StoreGameData storeGameData, uint? expireInMinutes);

    public void SetUserId(string nickname, ulong userId, uint? expireInMinutes);

    public StoreGameData GetStoreGameData(ulong appId);

    public ulong GetUserId(string nickname);
}