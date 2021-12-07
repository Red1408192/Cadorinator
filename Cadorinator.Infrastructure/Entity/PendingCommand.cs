using System;
using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class PendingCommand
    {
        public long PendingCommandId { get; set; }
        public long CommandId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
