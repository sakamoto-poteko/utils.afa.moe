using Moe.Afa.Utils.API.Services.Models.Steam;
using Moe.Afa.Utils.Common.Models;

namespace Moe.Afa.Utils.API.Services;

public interface ISteamService
{
    public Task<IList<OwnedGame>> GetOwnedGamesAsync(ulong userId);
    
    public Task<IList<SteamGameModel>> GetGameDetailsAsync(IList<ulong> gameIds);

    public Task<ulong> GetSteamIdByNicknameAsync(string nickname);
}