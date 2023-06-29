using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Moe.Afa.Utils.API.Services.Exceptions;
using Moe.Afa.Utils.API.Services.Models.PhoneNumber;
using Moe.Afa.Utils.API.Services.Models.PhoneNumber.Plocn;
using Moe.Afa.Utils.API.Settings;

namespace Moe.Afa.Utils.API.Services;

public class PlocnPhoneNumberLookupService : IChinaCellphoneNumberLookupService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PlocnPhoneNumberLookupService> _logger;
    private readonly CidLookupSettings _cidLookupSettings;
    private readonly string _appCode;

    public PlocnPhoneNumberLookupService(HttpClient httpClient, IOptions<CidLookupSettings> CidLookupSettings, ILogger<PlocnPhoneNumberLookupService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _appCode = CidLookupSettings.Value.PlocnAppCode;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("APPCODE", _appCode);
    }

    private readonly Regex _chinaPhoneNumberRegex = new(@"\+86(?<number>1\d\d\d\d\d\d\d\d\d\d)", RegexOptions.Compiled);

    public async Task<PhoneNumberInfo> GetPhoneNumberInfoAsync(string number)
    {
        var match = _chinaPhoneNumberRegex.Match(number);
        if (match.Success && match.Groups["number"].Success)
        {
            string phoneNum = match.Groups["number"].Value;
            var response = await _httpClient.GetAsync($"https://plocn.market.alicloudapi.com/plocn?n={phoneNum}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<PlocnResponse>();
                return json == null ? throw new InvokeExternalServiceException("invalid response") : PlocnResponseToPhoneNumberInfo(json);
            }
            else
            {
                throw new InvokeExternalServiceException("invalid response");
            }
        }
        else
        {
            throw new InvalidPhoneNumberException("The number is not a Chinese cell phone");
        }
    }

    private PhoneNumberInfo PlocnResponseToPhoneNumberInfo(PlocnResponse response)
    {
        return new()
        {
            City = response.City,
            Phone = response.Phone,
            Province = response.Province,
            ServiceProvider = response.Company,
        };
    }
}