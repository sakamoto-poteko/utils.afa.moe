using System.Text.Json.Serialization;

namespace Moe.Afa.Utils.Common.Models;

public class PotekoOfficeStateUpdateRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PotekoOfficeState State { get; set; }
    
    public DateTime Date { get; set; }
    
    /// <summary>
    /// base64(SHA-512(concat(password.UTF-8, (Now.EpochSeconds / 15).ToString().UTF-8))
    /// </summary>
    public string? OneTimePassword { get; set; }
}