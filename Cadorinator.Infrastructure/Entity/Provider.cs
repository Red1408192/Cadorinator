using System;
using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class Provider
    {
        public Provider()
        {
            ProjectionsSchedules = new HashSet<ProjectionsSchedule>();
            Theaters = new HashSet<Theater>();
        }

        public long ProviderId { get; set; }
        public string ProviderDomain { get; set; }
        public long ProviderSource { get; set; }
        public long CityId { get; set; }

        public virtual ProviderSource ProviderSourceNavigation { get; set; }
        public virtual City CityNavigation { get; set; }
        public virtual ICollection<ProjectionsSchedule> ProjectionsSchedules { get; set; }
        public virtual ICollection<Theater> Theaters { get; set; }
    }
}
