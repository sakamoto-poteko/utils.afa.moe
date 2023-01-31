using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moe.Afa.Utils.API.Models;
using Moe.Afa.Utils.Common.Models;

namespace Moe.Afa.Utils.API.Controllers;

[ApiController]
[Route("potekoOfficeStatus")]
public class PotekoOfficeStatusController : ControllerBase
{
    private const string TableName = "PotekoOfficeStatus";
    private readonly TableServiceClient _tableServiceClient;
    private readonly string _password;

    public PotekoOfficeStatusController(
        TableServiceClient tableServiceClient, IOptions<Settings.PotekoOfficeStatusSettings> settings)
    {
        _tableServiceClient = tableServiceClient;
        _password = settings.Value?.UpdatePassword ?? "dummy";
    }

    [Route("update")]
    [HttpPost]
    public async Task<IActionResult> UpdatePotekoOfficeStatus([FromBody] PotekoOfficeStateUpdateRequest request)
    {
        if (!VerifyPassword(request.OneTimePassword))
        {
            return Unauthorized();
        }

        await _tableServiceClient.CreateTableIfNotExistsAsync(TableName);
        var tableClient = _tableServiceClient.GetTableClient(TableName);

        await tableClient.UpsertEntityAsync(new PotekoOfficeStatus(request.Date, request.State));

        return NoContent();
    }

    [Route("{date}")]
    [HttpGet]
    public async Task<IActionResult> GetPotekoOfficeStatus([FromRoute] string date)
    {
        DateTime d;

        date = HttpUtility.UrlDecode(date);
        
        if (DateOnly.TryParse(date, out var dd))
        {
            d = dd.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        }
        else if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, DateTimeStyles.None, out d))
        {
            d = DateTime.SpecifyKind(d, DateTimeKind.Utc);
        }
        else
        {
            return NotFound();
        }

        d = DateTime.SpecifyKind(d, DateTimeKind.Utc);

        var dateStr = d.ToString("yyyy-MM-dd");

        var tableClient = _tableServiceClient.GetTableClient(TableName);
        var entity = await tableClient.GetEntityIfExistsAsync<PotekoOfficeStatus>(dateStr, dateStr);

        if (entity?.HasValue ?? false)
        {
            return Ok(new PotekoOfficeStateUpdateResponse()
            {
                State = entity.Value.State,
                Date = d,
            });
        }
        else
        {
            return Ok(new PotekoOfficeStateUpdateResponse()
            {
                State = PotekoOfficeState.Unknown,
                Date = d,
            });
        }
    }

    private bool VerifyPassword(string? oneTimePassword)
    {
        // base64(SHA-512(concat(password.UTF-8, (Now.EpochSeconds / 15).ToString().UTF-8))

        if (oneTimePassword == null)
        {
            return false;
        }

        using var sha512 = SHA512.Create();

        string Compute(long time)
        {
            return Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(time + _password)));
        }

        long seconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 15;
        return oneTimePassword == Compute(seconds)
               || oneTimePassword == Compute(seconds - 1)
               || oneTimePassword == Compute(seconds + 1);
    }
}