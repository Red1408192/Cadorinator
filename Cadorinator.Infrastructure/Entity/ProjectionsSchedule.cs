using Cadorinator.Infrastructure.Entity;
using System;
using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class ProjectionsSchedule
    {
        public ProjectionsSchedule()
        {
            Samples = new HashSet<Sample>();
        }

        public long ProjectionsScheduleId { get; set; }
        public long FilmId { get; set; }
        public long ThreaterId { get; set; }
        public long ProviderId { get; set; }
        public DateTime ProjectionTimestamp { get; set; }
        public string SourceEndpoint { get; set; }

        public virtual Film Film { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<Sample> Samples { get; set; }
    }
}
