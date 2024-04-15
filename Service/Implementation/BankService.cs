using AutoMapper;
using Repository.Abstraction;
using Repository.Entity;
using Repository.Implementation;
using Service.Model.Models.Request;
using Service.Model.Models.Response;
using Service.Model.StaticModels;

namespace Service.Implementation;

public class BankService : IBankService
{
    private readonly ILocationRepository locationRepository;
    private readonly IMapper mapper;

    public BankService(ILocationRepository locationRepository, IMapper mapper)
    {
        this.locationRepository=locationRepository;
        this.mapper=mapper;
    }

    public async Task AddBankLocation(GetLocationModel request, string bankName)
    {
        foreach (Result item in request.Results)
        {
            BankLocation location = new()
            {
                LastUpdatedDate = DateTime.UtcNow,
                CreationDate = DateTime.UtcNow,
                BankId = BanksDictionary.Banks[bankName],
                Latitude = item.Geometry.Location.Lat,
                Longitude = item.Geometry.Location.Lng,
                LocationName = item.Formatted_address
            };
            await locationRepository.AddAsync(location);
        }
    }

    public async Task<List<BankLocationResponseModel>> GetBankLocationsByName(int bankId)
    {
        IEnumerable<BankLocation> result = await locationRepository.GetAllAsync(x => x.BankId == bankId);

        try
        {
            mapper.Map<List<BankLocationResponseModel>>(result.ToList());
        }
        catch (Exception ex)
        {

            throw;
        }
        return mapper.Map<List<BankLocationResponseModel>>(result.ToList());
    }
}
