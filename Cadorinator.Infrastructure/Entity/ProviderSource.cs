using System;
using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class ProviderSource
    {
        public ProviderSource()
        {
            Providers = new HashSet<Provider>();
        }

        public long ProviderSourceId { get; set; }
        public string ProviderSourceName { get; set; }

        public virtual ICollection<Provider> Providers { get; set; }
    }
}
