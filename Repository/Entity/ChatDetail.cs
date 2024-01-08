using System;
using System.Collections.Generic;

namespace Repository.Entity
{
    public partial class ChatDetail
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public DateTime CreationDate { get; set; }
        public long UserActivityHistoryId { get; set; }
        public long MessageExternalId { get; set; }

        public virtual UsersActivityHistory UserActivityHistory { get; set; }
    }
}
