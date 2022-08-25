using Moe.Afa.Utils.API.Services.Models.PhoneNumber;

namespace Moe.Afa.Utils.API.Services;

public interface IChinaCellphoneNumberLookupService
{
    public Task<PhoneNumberInfo> GetPhoneNumberInfoAsync(string number);
}