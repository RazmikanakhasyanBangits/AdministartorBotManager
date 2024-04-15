namespace Service.Model.Models.Request;

public class GetLocationByNameRequest
{
    public string LocationName { get; set; }
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
}
