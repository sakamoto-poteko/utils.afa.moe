using Moe.Afa.Utils.API.Services.Models.PhoneNumber;
namespace Moe.Afa.Utils.API.Services;

public interface IChinaLandlineNumberLookupService
{
    public Task<PhoneNumberInfo> GetPhoneNumberInfoAsync(string number);
}

public class ChinaLandlineNumberLookupService : IChinaLandlineNumberLookupService
{
    public Task<PhoneNumberInfo> GetPhoneNumberInfoAsync(string number)
    {
        return Task.FromResult(new PhoneNumberInfo());
    }
}
