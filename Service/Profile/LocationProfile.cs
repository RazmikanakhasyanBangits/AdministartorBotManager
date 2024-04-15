using Repository.Entity;
using Service.Model.Models.Response;

namespace Service.Profile;

public class LocationProfile : AutoMapper.Profile
{
    public LocationProfile()
    {
        CreateMap<BankLocationResponseModel, BankLocation>().ReverseMap();

    }
}
