using System;
using System.Collections.Generic;

namespace Repository.Entity
{
    public partial class UserRole
    {
        public UserRole()
        {
            UsersActivityHistories = new HashSet<UsersActivityHistory>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime DeletionDate { get; set; }

        public virtual ICollection<UsersActivityHistory> UsersActivityHistories { get; set; }
    }
}
