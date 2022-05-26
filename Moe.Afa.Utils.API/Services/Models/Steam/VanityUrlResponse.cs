using System.Text.Json.Serialization;

namespace Moe.Afa.Utils.API.Services.Models.Steam;

public class VanityUrlResponse
{
    [JsonPropertyName("steamid")]
    public ulong SteamId { get; set; }

    [JsonPropertyName("success")]
    public int Success { get; set; }
}