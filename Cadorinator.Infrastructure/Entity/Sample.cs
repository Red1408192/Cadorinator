using System;
using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class Sample
    {
        public long SampleId { get; set; }
        public DateTime SampleTimestamp { get; set; }
        public long ProjectionsScheduleId { get; set; }
        public long BoughtSeats { get; set; }
        public long LockedSeats { get; set; }
        public long ReservedSeats { get; set; }
        public long QuarantinedSeats { get; set; }
        public long TotalSeats { get; set; }

        public virtual ProjectionsSchedule ProjectionsSchedule { get; set; }
    }
}
