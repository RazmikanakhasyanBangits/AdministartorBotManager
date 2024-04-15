using Service.Model.Models.Response;

namespace Repository.Abstraction;

public interface ILocationService
{
    Task<string> GetLocationsAsync(string selection);
    Task<string> UpdateAllBankLocationsAsync();
}
