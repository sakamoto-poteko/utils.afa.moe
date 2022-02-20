using Microsoft.AspNetCore.Mvc;
using Moe.Afa.Utils.API.Services;
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
    public async Task<ActionResult<IEnumerable<SteamGameModel>>> GetOwnedGamesIntersection(
        [FromBody] OwnedSteamGamesIntersectionRequestModel requestModel)
    {
        ulong user1Id, user2Id;
        
        if (requestModel.User1Id == null)
        {
            if (requestModel.User1NickName == null)
            {
                return BadRequest("User 1's ID or nickname must be supplied");
            }
            else
            {
                user1Id = await _steamService.GetSteamIdByNicknameAsync(requestModel.User1NickName);
            }
        }
        else
        {
            user1Id = requestModel.User1Id.Value;
        }

        if (requestModel.User2Id == null)
        {
            if (requestModel.User2NickName == null)
            {
                return BadRequest("User 2's ID or nickname must be supplied");
            }
            else
            {
                user2Id = await _steamService.GetSteamIdByNicknameAsync(requestModel.User2NickName);
            }
        }
        else
        {
            user2Id = requestModel.User2Id.Value;
        }
        
        var user1Games = await _steamService.GetOwnedGamesAsync(user1Id);
        var user2Games = await _steamService.GetOwnedGamesAsync(user2Id);
        var user1PlayedGames = user1Games.Where(game => game.Playtime > 0).Select(game => game.AppId);
        var user2PlayedGames = user2Games.Where(game => game.Playtime > 0).Select(game => game.AppId);
        var intersection = user1PlayedGames.Intersect(user2PlayedGames).OrderBy(id => id).ToList();

        var gameDetails = await _steamService.GetGameDetailsAsync(intersection);
        return Ok(gameDetails);
    }
}