using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;
using Moe.Afa.Utils.Common.Models;

namespace Moe.Afa.Utils.API.Models;

public class PotekoOfficeStatus : ITableEntity
{
    public PotekoOfficeStatus()
    {
    }

    public PotekoOfficeStatus(DateTime date, PotekoOfficeState state)
    {
        Date = new DateTime(date.Ticks, DateTimeKind.Utc);
        State = state;
    }

    private DateTime _date = DateTime.MinValue;

    public string PartitionKey
    {
        get => _date.ToString("yyyy-MM-dd");
        set => _date = DateTime.SpecifyKind(DateTime.ParseExact(value, "yyyy-MM-dd", null), DateTimeKind.Utc);
    }
    
    public string RowKey
    {
        get => _date.ToString("yyyy-MM-dd");
        set => _date = DateTime.SpecifyKind(DateTime.ParseExact(value, "yyyy-MM-dd", null), DateTimeKind.Utc);
    }

    [JsonIgnore]
    [IgnoreDataMember]
    public DateTime Date
    {
        get => _date;
        set => _date = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
    }

    
    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PotekoOfficeState State { get; set; }
}