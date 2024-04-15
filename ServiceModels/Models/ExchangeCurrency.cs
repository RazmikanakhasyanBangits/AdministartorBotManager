namespace Service.Model.Models;

public class ExchangeCurrency
{
    public int BankId { get; set; }
    public string Currency { get; set; }
    public decimal BuyValue { get; set; }
    public decimal SellValue { get; set; }
}
