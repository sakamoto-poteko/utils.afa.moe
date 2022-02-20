using Microsoft.Extensions.Options;
using Moe.Afa.Utils.API.Services.SteamApiModels;
using Moe.Afa.Utils.API.Settings;
using Moe.Afa.Utils.Common.Models;

namespace Moe.Afa.Utils.API.Services;

public class SteamService : ISteamService
{
    private readonly HttpClient _httpClient;
    private readonly ISteamCacheManager _steamCacheManager;
    private readonly ILogger<SteamService> _logger;
    private readonly string _steamKey;

    public SteamService(
        HttpClient httpClient, IOptions<SteamSettings> steamSettings, ISteamCacheManager steamCacheManager,
        ILogger<SteamService> logger)
    {
        _httpClient = httpClient;
        _steamCacheManager = steamCacheManager;
        _steamKey = steamSettings.Value.ApiKey;
        _logger = logger;
    }

    public async Task<IList<OwnedGame>> GetOwnedGamesAsync(ulong userId)
    {
        var response = await _httpClient.GetAsync(
            $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_steamKey}&steamid={userId}&include_played_free_games=1&format=json");

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<SteamApiResponse<OwnedGamesResponse>>();

        return json.Response.Games.ToList();
    }

    public async Task<IList<SteamGameModel>> GetGameDetailsAsync(IList<ulong> gameIds)
    {
        List<SteamGameModel> steamGames = new();

        foreach (var gameId in gameIds)
        {
            StoreGameData cachedData;

            try
            {
                cachedData = _steamCacheManager.GetStoreGameData(gameId);
            }
            catch (KeyNotFoundException)
            {
                var response = await _httpClient.GetAsync($"https://store.steampowered.com/api/appdetails?appids={gameId}");
                response.EnsureSuccessStatusCode();

                var games = await response.Content.ReadFromJsonAsync<IDictionary<ulong, StoreGame>>();

                var game = games.Single().Value;
                if (!game.Success)
                {
                    game.Data = new()
                    {
                        Name = "[Retired]", 
                    };
                }

                cachedData = game.Data;
                _steamCacheManager.SetStoreGameData(gameId, cachedData, 30 * 24 * 60);
            }

            steamGames.Add(new()
            {
                SteamAppId = cachedData.SteamAppId,
                Name = cachedData.Name,
                ShortDescription = cachedData.ShortDescription,
                RequiredAge = cachedData.RequiredAge,
                Website = cachedData.Website,
                HeaderImage = cachedData.HeaderImage,
                Developers = cachedData.Developers,
                Publishers = cachedData.Publishers,
            });
        }

        return steamGames;
    }

    public async Task<ulong> GetSteamIdByNicknameAsync(string nickname)
    {
        ulong userId;
        try
        {
            userId = _steamCacheManager.GetUserId(nickname);
        }
        catch (KeyNotFoundException)
        {
            var response = await _httpClient.GetAsync(
                $"https://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={_steamKey}&vanityurl={nickname}");

            response.EnsureSuccessStatusCode();

            var vanityUrl = await response.Content.ReadFromJsonAsync<SteamApiResponse<VanityUrlResponse>>();

            userId = vanityUrl.Response.SteamId;
            _steamCacheManager.SetUserId(nickname, userId, 30 * 24 * 60);
        }

        return userId;
    }
}