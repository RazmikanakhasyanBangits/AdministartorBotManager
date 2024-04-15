using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Entity;
using Service.Model.Models.Request;
using Service.Model.Models.Response;
using Service.Model.StaticModels;
using Service.Services.DataScrapper.Impl;
using Service.Services.DataScrapper.Implementation;
using System.Text;

namespace Service.Implementation;

public class LocationService : ILocationService
{

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IBankService bankService;
    public LocationService(IBankService bankService, IServiceScopeFactory scopeFactory)
    {
        this.bankService=bankService;
        _scopeFactory=scopeFactory;
    }
    public Dictionary<string, ILocationService> Dictionary()
    {
        var dict = new Dictionary<string, ILocationService>();
        IServiceScope scope = _scopeFactory.CreateScope();
        IServiceProvider scopedServices = scope.ServiceProvider;
        ILocationService locationService = scopedServices.GetRequiredService<ILocationService>();
        IBankService bankService = scopedServices.GetRequiredService<IBankService>();

        dict =new Dictionary<string, ILocationService>
                {
                    {  nameof(AmeriaBankDataScrapper) , new AmeriaBankDataScrapper(locationService, bankService) },
                    {  nameof(EvocaBankDataScrapper) , new EvocaBankDataScrapper(locationService, bankService) },
                    {  nameof(AcbaBankDataScrapper), new AcbaBankDataScrapper(locationService, bankService) },
                    {  nameof(InecoBankDataScrapper), new InecoBankDataScrapper(locationService,bankService) },
                    {  nameof(UniBankDataScrapper) , new UniBankDataScrapper(locationService, bankService) },
                }.ToDictionary();
        return dict;
    }
    public async Task<string> GetLocationsAsync(string selection)
    {
        List<BankLocationResponseModel> locations = await bankService.GetBankLocationsByName(BanksDictionary.Banks[selection]);

        if (locations is null || !locations.Any())
        {
            string result = await Dictionary()[selection].GetLocationsAsync(selection);
            return result;

        }
        StringBuilder builder = new();
        foreach (BankLocationResponseModel item in locations)
        {
            _ = builder.AppendLine($"{item.LocationName}");
            _ = builder.AppendLine($"Թարմացվել է` {item.LastUpdatedDate:G}");
            _ = builder.AppendLine("--------------------------------------------");
        }
        return builder.ToString();
    }

    public async Task<string> UpdateAllBankLocationsAsync()
    {
        StringBuilder builder = new();
        foreach (var item in BanksDictionary.Banks)
        {

            List<BankLocationResponseModel> locations = await bankService.GetBankLocationsByName(BanksDictionary.Banks[item.Key]);

            if (locations is null || !locations.Any())
            {
                await Dictionary()[item.Key].GetLocationsAsync(item.Key);
            }
            foreach (BankLocationResponseModel location in locations)
            {
                _ = builder.AppendLine($"{location.LocationName}");
                _ = builder.AppendLine($"Թարմացվել է` {location.LastUpdatedDate:G}");
                _ = builder.AppendLine("--------------------------------------------");
            }
        }
        return builder.ToString();
    }


}
