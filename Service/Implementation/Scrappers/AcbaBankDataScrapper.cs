// Ignore Spelling: Acba

using Microsoft.Extensions.Configuration;
using Service.Model.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Repository.Abstraction;
using HtmlAgilityPack;
using Service.Model.Models.Request;
using Service.Helpers;

namespace Service.Services.DataScrapper.Implementation
{
    public class AcbaBankDataScrapper: ILocationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILocationService _locationService;
        private readonly IBankService bankService;
        public AcbaBankDataScrapper(ILocationService locationService, IBankService bankService)
        {
            _configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
            _locationService = locationService;
            this.bankService=bankService;
        }

        public int Id => 3;

        public IEnumerable<ExchangeCurrency> Get()
        {
            HttpClient client = new();
            string html = client.GetStringAsync("https://www.acba.am/en")
                .GetAwaiter()
                .GetResult();

            HtmlDocument doc = new();
            doc.LoadHtml(html);
            IEnumerable<HtmlNode> nodes = doc.DocumentNode.SelectNodes("//div[@class='simple_price-row']").Skip(1);

            List<ExchangeCurrency> models = new();
            foreach (HtmlNode item in nodes)
            {
                HtmlNode[] childs = item.ChildNodes.Where(_ => _.Name == "div").ToArray();

                try
                {
                    ExchangeCurrency model = new()
                    {
                        Currency = childs[0].InnerText.Trim(),
                        BuyValue = Convert.ToDecimal(childs[1].InnerText.Trim(), CultureInfo.InvariantCulture),
                        SellValue = Convert.ToDecimal(childs[2].InnerText.Trim(), CultureInfo.InvariantCulture),
                        BankId = Id
                    };
                    models.Add(model);
                }
                catch { }

            }
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

            return nameof(AcbaBankDataScrapper).Replace("DataScrapper", "");
        }

        public Task<string> UpdateAllBankLocationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
