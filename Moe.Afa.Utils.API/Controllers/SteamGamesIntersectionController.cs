using Microsoft.AspNetCore.Mvc;
using Moe.Afa.Utils.API.Services;
using Moe.Afa.Utils.API.Services.SteamApiModels;
using Moe.Afa.Utils.Common.Models;

namespace Moe.Afa.Utils.API.Controllers;

[ApiController]
[Route("steamgames")]
public class SteamGamesIntersectionController : ControllerBase
{
    private readonly ISteamService _steamService;
    private readonly ILogger<SteamGamesIntersectionController> _logger;

    public SteamGamesIntersectionController(ISteamService steamService, ILogger<SteamGamesIntersectionController> logger)
    {
        _steamService = steamService;
        _logger = logger;
    }

    [HttpPost("intersection")]
    public async Task<IEnumerable<SteamGameModel>> GetOwnedGamesIntersection(
        [FromBody] OwnedSteamGamesIntersectionRequestModel ownedSteamGamesIntersectionRequestModel)
    {
        var user1Games = await _steamService.GetOwnedGamesAsync(ownedSteamGamesIntersectionRequestModel.User1Id);
        var user2Games = await _steamService.GetOwnedGamesAsync(ownedSteamGamesIntersectionRequestModel.User2Id);
        var user1PlayedGames = user1Games.Where(game => game.Playtime > 10).Select(game => game.AppId);
        var user2PlayedGames = user2Games.Where(game => game.Playtime > 10).Select(game => game.AppId);
        var intersection = user1PlayedGames.Intersect(user2PlayedGames).OrderBy(id => id).ToList();

        var gameDetails = await _steamService.GetGameDetailsAsync(intersection);
        return gameDetails;
    }
}