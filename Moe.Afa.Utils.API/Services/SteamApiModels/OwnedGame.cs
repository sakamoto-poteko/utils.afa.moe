using System.Text.Json.Serialization;

namespace Moe.Afa.Utils.API.Services.SteamApiModels;

public class OwnedGame
{
    [JsonPropertyName("appid")]
    public ulong AppId { get; set; }

    [JsonPropertyName("playtime_forever")]
    public ulong Playtime { get; set; }

    [JsonPropertyName("playtime_windows_forever")]
    public ulong PlaytimeWindows { get; set; }

    [JsonPropertyName("playtime_mac_forever")]
    public ulong PlaytimeMac { get; set; }

    [JsonPropertyName("playtime_linux_forever")]
    public ulong PlaytimeLinux { get; set; }
}

public class OwnedGamesResponse
{
    [JsonPropertyName("response")]
    public OwnedGamesResponseBody Response { get; set; }
    
    public class OwnedGamesResponseBody
    {
        [JsonPropertyName("game_count")]
        public ulong GameCount { get; set; }

        [JsonPropertyName("games")]
        public IList<OwnedGame> Games { get; set; }
    }
}

public class OwnedGameAppIdComparer : IEqualityComparer<OwnedGame>
{
    public bool Equals(OwnedGame? x, OwnedGame? y)
    {
        return x?.AppId == y?.AppId;
    }

    public int GetHashCode(OwnedGame obj)
    {
        return HashCode.Combine(obj.AppId, obj.Playtime, obj.PlaytimeWindows, obj.PlaytimeMac, obj.PlaytimeLinux);
    }
}