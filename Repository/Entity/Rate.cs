using System;
using System.Collections.Generic;

namespace Repository.Entity
{
    public partial class Rate
    {
        public int Id { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
        public int Iteration { get; set; }
        public int BankId { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Currency FromCurrencyNavigation { get; set; }
        public virtual Currency ToCurrencyNavigation { get; set; }
    }
}
