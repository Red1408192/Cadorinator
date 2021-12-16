using System;
using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class City
    {
        public City()
        {
            Providers = new HashSet<Provider>();
        }

        public long CityId { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<Provider> Providers { get; set; }
    }
}
