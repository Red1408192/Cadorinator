using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class Theater
    {
        public long TheaterId { get; set; }
        public long ProviderId { get; set; }
        public string TheaterName { get; set; }

        public virtual Provider ProviderNavigation { get; set; }
    }
}
