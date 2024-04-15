// Ignore Spelling: Ineco Online

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repository.Abstraction;
using Service.Helpers;
using Service.Model.Models;
using Service.Model.Models.Request;
using System.Text;

namespace Service.Services.DataScrapper.Impl;
public class InecoBankDataScrapper : ILocationService
{ 
    private readonly IConfiguration _configuration;
    private readonly ILocationService _locationService;
    private readonly IBankService bankService;
    public InecoBankDataScrapper(ILocationService locationService, IBankService bankService)
    {
        _configuration = new ConfigurationBuilder()
                         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                         .AddJsonFile("appsettings.json")
                         .Build();
        _locationService = locationService;
        this.bankService=bankService;
    }


    public int Id => 4;

    public IEnumerable<ExchangeCurrency> Get()
    {
        IEnumerable<ExchangeCurrency> models = null;
        HttpClient client = new();
        try
        {
            string json = client.GetStringAsync("https://www.inecobank.am/api/rates/")
                .GetAwaiter()
                .GetResult();
            models = JsonConvert.DeserializeObject<InecoDataModel>(json).Items
                .Where(_ => _.Card.Buy.HasValue && _.Card.Sell.HasValue)
                .Select(_ => new ExchangeCurrency
                {
                    BuyValue = (decimal)_.Card.Buy,
                    SellValue = (decimal)_.Card.Sell,
                    Currency = _.Code,
                    BankId = Id
                });

        }
        catch { }

        return models;
    }

    public async Task<string> GetLocationsAsync(string selection)
    {
        GetLocationModel data = await LocationHelper.GetLocationsByNameAsync(new GetLocationByNameRequest
        {
            ApiKey = _configuration["Location:ApiKey"],
            BaseUrl = _configuration["Location:BaseUrl"],
            LocationName = ToString()
        });
        await bankService.AddBankLocation(data, selection);
        StringBuilder builder = new();
        foreach (Result item in data.Results)
        {
            _ = builder.AppendLine($"{item.Formatted_address}");

            _ = builder.AppendLine($"Թարմացվել է` {DateTime.UtcNow:G}");
            _ = builder.AppendLine("--------------------------------------------");
        }
        return builder.ToString();
    }

    public override string ToString()
    {
        return nameof(InecoBankDataScrapper).Replace("DataScrapper", "");
    }

    public Task<string> UpdateAllBankLocationsAsync()
    {
        throw new NotImplementedException();
    }

    public partial class InecoDataModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("cash")]
        public Card Cash { get; set; }

        [JsonProperty("cashless")]
        public Card Cashless { get; set; }

        [JsonProperty("online")]
        public Card Online { get; set; }

        [JsonProperty("cb")]
        public Card Cb { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }
    }

    public partial class Card
    {
        [JsonProperty("buy")]
        public double? Buy { get; set; }

        [JsonProperty("sell")]
        public double? Sell { get; set; }
    }
}
