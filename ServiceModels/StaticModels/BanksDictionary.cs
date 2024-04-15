namespace Service.Model.StaticModels;

public static class BanksDictionary
{
    public static Dictionary<string, int> Banks { get; set; } = new()
    {
        { "AmeriaBankDataScrapper",1},
        { "EvocaBankDataScrapper",2},
        { "AcbaBankDataScrapper",3},
        { "InecoBankDataScrapper",4},
        { "UniBankDataScrapper",5},
    };
}
