using System;
using System.Collections.Generic;

#nullable disable

namespace Cadorinator.Infrastructure.Entity
{
    public partial class Film
    {
        public Film()
        {
            ProjectionsSchedules = new HashSet<ProjectionsSchedule>();
        }

        public long FilmId { get; set; }
        public string FilmName { get; set; }
        public DateTime FirstProjectionDate { get; set; }

        public virtual ICollection<ProjectionsSchedule> ProjectionsSchedules { get; set; }
    }
}
