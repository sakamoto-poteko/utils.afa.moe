using Moe.Afa.Utils.API.Services.Models.Plocn;

namespace Moe.Afa.Utils.API.Services;

public interface IPlocnPhoneNumberLookupService
{
    public Task<PlocnResponse> GetPhoneNumberInfoAsync(string number);
}