using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Moe.Afa.Utils.API.Services.Exceptions;
using Moe.Afa.Utils.API.Services.Models.Plocn;
using Moe.Afa.Utils.API.Settings;

namespace Moe.Afa.Utils.API.Services;

public class PlocnPhoneNumberLookupService : IPlocnPhoneNumberLookupService
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

    private readonly Regex _chinaPhoneNumberRegex = new Regex(@"\+86(?<number>1\d\d\d\d\d\d\d\d\d\d)", RegexOptions.Compiled);
    
    public async Task<PlocnResponse> GetPhoneNumberInfoAsync(string number)
    {
        var match = _chinaPhoneNumberRegex.Match(number);
        if (match.Success && match.Groups["number"].Success)
        {
            string phoneNum = match.Groups["number"].Value;
            var response = await _httpClient.GetAsync($"https://plocn.market.alicloudapi.com/plocn?n={phoneNum}");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<PlocnResponse>();
            return json ?? throw new InvokeExternalServiceException("invalid response");
        }
        else
        {
            throw new InvalidPhoneNumberException("The number is not a Chinese cell phone");
        }
    }
}