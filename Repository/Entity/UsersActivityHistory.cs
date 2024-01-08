using System;
using System.Collections.Generic;

namespace Repository.Entity
{
    public partial class UsersActivityHistory
    {
        public UsersActivityHistory()
        {
            ChatDetails = new HashSet<ChatDetail>();
        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UserExternalId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Bio { get; set; }
        public string Description { get; set; }
        public short RoleId { get; set; }
        public short StatusId { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual UserStatus Status { get; set; }
        public virtual ICollection<ChatDetail> ChatDetails { get; set; }
    }
}
