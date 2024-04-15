namespace Repository.Entity;

public class BankLocation
{
    public long Id { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string LocationName { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public int BankId { get; set; }
    public Bank Bank { get; set; }
}
