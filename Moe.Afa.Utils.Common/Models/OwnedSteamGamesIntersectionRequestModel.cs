using System.ComponentModel.DataAnnotations;

namespace Moe.Afa.Utils.Common.Models;

public class OwnedSteamGamesIntersectionRequestModel
{
    public ulong? User1Id { get; set; }
    
    public string? User1NickName { get; set; }
    
    public ulong? User2Id { get; set; }
    
    public string? User2NickName { get; set; }

}