using System.ComponentModel.DataAnnotations;

namespace Moe.Afa.Utils.Common.Models;

public class OwnedSteamGamesIntersectionRequestModel
{
    [Required]
    public ulong User1Id { get; set; }
    
    [Required]
    public ulong User2Id { get; set; }
}