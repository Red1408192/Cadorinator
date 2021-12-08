using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadorinator.Service.Model
{
    public class TheaterSeats
    {
        public List<Seat> Bought { get; set; }
        public List<Seat> Locked { get; set; }
        public List<Seat> Reserved { get; set; }
        public List<Seat> Quarantined { get; set; }
        public List<Seat> Mine { get; set; }
        public int Total { get; set; }
        public int Available { get; set; }
        public int Busy { get; set; }
    }
}
