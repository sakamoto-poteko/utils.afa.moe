using System.Text.Json.Serialization;

namespace Moe.Afa.Utils.Common.Models;

public class PotekoOfficeStateUpdateResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PotekoOfficeState State { get; set; }
    
    public DateTime Date { get; set; }
}