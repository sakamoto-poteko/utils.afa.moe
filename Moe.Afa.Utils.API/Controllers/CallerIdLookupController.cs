using Microsoft.AspNetCore.Mvc;
using Moe.Afa.Utils.API.Services;

namespace Moe.Afa.Utils.API.Controllers;

[ApiController]
[Route("cidlookup")]
public class CallerIdLookupController : ControllerBase
{
    private readonly IPlocnPhoneNumberLookupService _plocnPhoneNumberLookupService;

    public CallerIdLookupController(IPlocnPhoneNumberLookupService plocnPhoneNumberLookupService)
    {
        _plocnPhoneNumberLookupService = plocnPhoneNumberLookupService;
    }

    [Route("lookup/{number}")]
    [HttpGet]
    public async Task<ActionResult<string>> GetCallerId(string number)
    {
        if (number.StartsWith("+86"))
        {
            if (number.Length == 14)
            {
                // Cellphone in China
                var result = await _plocnPhoneNumberLookupService.GetPhoneNumberInfoAsync(number);
                return $"{result.City}({result.Province}){result.Company}";
            }
        }

        return "Unknown";
    }
}
