using System;
using System.Collections.Generic;

namespace Repository.Entity
{
    public partial class Location
    {
        public long Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string LocationName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int BankId { get; set; }

        public virtual Bank Bank { get; set; }
    }
}
