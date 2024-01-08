using System;
using System.Collections.Generic;

namespace Repository.Entity
{
    public partial class Bank
    {
        public Bank()
        {
            Locations = new HashSet<Location>();
            Rates = new HashSet<Rate>();
        }

        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankUrl { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
