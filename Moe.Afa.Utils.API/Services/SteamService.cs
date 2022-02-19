using Microsoft.Extensions.Options;
using Moe.Afa.Utils.API.Services.SteamApiModels;
using Moe.Afa.Utils.API.Settings;
using Moe.Afa.Utils.Common.Models;

namespace Moe.Afa.Utils.API.Services;

public class SteamService : ISteamService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SteamService> _logger;
    private readonly string _steamKey;

    public SteamService(HttpClient httpClient, IOptions<SteamSettings> steamSettings, ILogger<SteamService> logger)
    {
        _httpClient = httpClient;
        _steamKey = steamSettings.Value.ApiKey;
        _logger = logger;
    }

    public async Task<IList<OwnedGame>> GetOwnedGamesAsync(ulong userId)
    {
        var response = await _httpClient.GetAsync(
            $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_steamKey}&steamid={userId}&include_played_free_games=1&format=json");

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<OwnedGamesResponse>();

        return json.Response.Games.ToList();
    }

    public async Task<IList<SteamGameModel>> GetGameDetailsAsync(IList<ulong> gameIds)
    {
        List<SteamGameModel> steamGames = new();

        foreach (var gameId in gameIds)
        {
            var response = await _httpClient.GetAsync($"https://store.steampowered.com/api/appdetails?appids={gameId}");
            response.EnsureSuccessStatusCode();

            var games = await response.Content.ReadFromJsonAsync<IDictionary<ulong, StoreGame>>();
            
            var data = games.Single().Value.Data;

            steamGames.Add(new()
            {
                SteamAppId = data.SteamAppId,
                Name = data.Name,
                ShortDescription = data.ShortDescription,
                RequiredAge = data.RequiredAge,
                Website = data.Website,
                HeaderImage = data.HeaderImage,
                Developers = data.Developers,
                Publishers = data.Publishers,
            });
        }

        return steamGames;
    }
}