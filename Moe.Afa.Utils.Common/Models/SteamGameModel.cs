namespace Moe.Afa.Utils.Common.Models;

public class SteamGameModel
{
    public string Name { get; set; }

    public ulong SteamAppId { get; set; }

    public uint RequiredAge { get; set; }
        
    public string ShortDescription { get; set; }

    public string HeaderImage { get; set; }

    public string Website { get; set; }
        
    public List<string> Developers { get; set; } = new();

    public List<string> Publishers { get; set; } = new();
}

