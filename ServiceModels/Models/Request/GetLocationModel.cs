namespace Service.Model.Models.Request;
public class GetLocationModel
{
    public List<Result> Results { get; set; }
    public string Status { get; set; }
}
public class Result
{
    public string Business_status { get; set; }
    public string Formatted_address { get; set; }
    public string Name { get; set; }
    public GeometryModel Geometry { get; set; }
}
public class GeometryModel
{
    public LocationModel Location { get; set; }
    public ViewPort Viewport { get; set; }
}
public class LocationModel
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}
public class ViewPort
{
    public NortheastModel northeast { get; set; }
    public SouthwestModel southwest { get; set; }
}
public class NortheastModel
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}
public class SouthwestModel
{
    public double lat { get; set; }
    public double lng { get; set; }
}
