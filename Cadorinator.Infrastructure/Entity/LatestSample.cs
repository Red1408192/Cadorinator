#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class LatestSample
    {
        public string FilmName { get; set; }
        public string ProviderDomain { get; set; }
        public string CityName { get; set; }
        public string TheaterName { get; set; }
        public string Date { get; set; }
        public string Time  { get; set; }
        public int Total { get; set; }
        public int Bought { get; set; }
        public int Locked { get; set; }
        public int Quarantined { get; set; }
        public string Eta { get; set; }
        public int LatestSampleId { get; set; }
    }
}
