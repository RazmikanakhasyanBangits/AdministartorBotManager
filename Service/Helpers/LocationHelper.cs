using Newtonsoft.Json;
using Service.Model.Models.Request;
using System.Net;

namespace Service.Helpers;

public static class LocationHelper
{
    public static async Task<GetLocationModel> GetLocationsByNameAsync(GetLocationByNameRequest model)
    {
        string requestUrl = $"{model.BaseUrl}{WebUtility.UrlEncode(model.LocationName)}&key={model.ApiKey}";
        try
        {
            using HttpClient client = new();
            string json = await client.GetStringAsync(requestUrl);

            GetLocationModel data = JsonConvert.DeserializeObject<GetLocationModel>(json);


            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new GetLocationModel();
        }
    }
}
