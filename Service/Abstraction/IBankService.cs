using Service.Model.Models.Request;
using Service.Model.Models.Response;

namespace Repository.Abstraction;

public interface IBankService
{
    Task AddBankLocation(GetLocationModel request, string bankName);
    Task<List<BankLocationResponseModel>> GetBankLocationsByName(int bankId);
}
