using System.Text.Json.Serialization;

namespace Moe.Afa.Utils.API.Services.SteamApiModels;

public class StoreGame
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public StoreGameData Data { get; set; }
}

public class StoreGameData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("steam_appid")]
    public ulong SteamAppId { get; set; }

    [JsonPropertyName("required)age")]
    public uint RequiredAge { get; set; }
        
    [JsonPropertyName("detailed_description")]
    public string DetailedDescription { get; set; }

    [JsonPropertyName("about_the_game")]
    public string AboutTheGame { get; set; }

    [JsonPropertyName("short_description")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("header_image")]
    public string HeaderImage { get; set; }

    [JsonPropertyName("website")]
    public string Website { get; set; }
        
    [JsonPropertyName("developers")]
    public List<string> Developers { get; set; } = new();

    [JsonPropertyName("publishers")]
    public List<string> Publishers { get; set; } = new();
}