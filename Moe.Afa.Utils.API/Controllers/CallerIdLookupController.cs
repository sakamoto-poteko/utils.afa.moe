using Microsoft.AspNetCore.Mvc;
using Moe.Afa.Utils.API.Services;
using Moe.Afa.Utils.API.Services.Models.PhoneNumber;

namespace Moe.Afa.Utils.API.Controllers;

[ApiController]
[Route("cidlookup")]
public class CallerIdLookupController : ControllerBase
{
    private readonly IChinaCellphoneNumberLookupService _plocnPhoneNumberLookupService;
    private readonly IChinaLandlineNumberLookupService landlineNumberLookupService;

    public CallerIdLookupController(IChinaCellphoneNumberLookupService cellphoneNumberLookupService, IChinaLandlineNumberLookupService landlineNumberLookupService)
    {
        _plocnPhoneNumberLookupService = cellphoneNumberLookupService;
        this.landlineNumberLookupService = landlineNumberLookupService;
    }

    [Route("lookup")]
    [HttpGet]
    public async Task<ActionResult<string>> GetCallerId([FromQuery] string number)
    {
        if (number.StartsWith("+86"))
        {
            if (number.Length == 14) // cellphone
            {
                // Cellphone in China
                var result = await _plocnPhoneNumberLookupService.GetPhoneNumberInfoAsync(number);
                return Ok(PhoneNumberInfoToString(result));
            }
            else
            {
                // Landline
                var result = await landlineNumberLookupService.GetPhoneNumberInfoAsync(number);
                return Ok(PhoneNumberInfoToString(result));
            }
        }

        return Ok("Unknown");
    }

    private string PhoneNumberInfoToString(PhoneNumberInfo phoneNumberInfo)
    {
        return $"{phoneNumberInfo.City}({phoneNumberInfo.Province}){phoneNumberInfo.ServiceProvider}";
    }
}
