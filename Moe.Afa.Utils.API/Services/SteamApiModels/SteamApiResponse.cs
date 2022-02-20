#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Moe.Afa.Utils.API.Services.SteamApiModels;

public class SteamApiResponse<T>
{
    [JsonPropertyName("response")]
    public T Response { get; set; }
}