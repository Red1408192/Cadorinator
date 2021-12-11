using Cadorinator.Infrastructure.Entity;
using Cadorinator.ServiceContract.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadorinator.Infrastructure
{
    public interface IDALService
    {
        Task AddSchedule(ProjectionsSchedule schedule);
        Task<IEnumerable<Provider>> ListProvidersAsync(long? providerSource = null);
        Task LoadSample(long projectionsScheduleId, long boughtSeats, long lockedSeats, long reservedSeats, long quarantinedSeats, long totalSeats);
        Task<Film> UpselectFilm(string filmName, DateTimeOffset projectionScehdule);
        Task<IEnumerable<ProjectionsSchedule>> ListProjectionsScheduleAsync(TimeSpan timeSpan);
    }

    public class DalService : IDALService
    {
        private readonly IDbContextFactory<MainContext> dbContextFactory;

        public DalService(IDbContextFactory<MainContext> dbContext, ICadorinatorSettings settings)
        {
            this.dbContextFactory = dbContext;
        }

        public async Task<IEnumerable<Provider>> ListProvidersAsync(long? providerSource = null)
        {
            using (var db = dbContextFactory.Create())
            {
                return await db.Providers
                    .Where(x => x.ProviderSource == (providerSource?? x.ProviderSource))
                    .ToListAsync();
            }
        }

        public async Task AddSchedule(ProjectionsSchedule schedule)
        {
            using (var db = dbContextFactory.Create())
            {
                if (await db.ProjectionsSchedules.AnyAsync(x => x.FilmId == schedule.FilmId && x.ProjectionTimestamp == schedule.ProjectionTimestamp && x.ProviderId == x.ProviderId)) return;
                await db.ProjectionsSchedules
                    .AddAsync(schedule);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProjectionsSchedule>> GetActiveSchedules()
        {
            using (var db = dbContextFactory.Create())
            {
                return await db.ProjectionsSchedules
                    .Where(x => x.ProjectionTimestamp > DateTime.UtcNow)
                    .Include(b => b.Film)
                    .Include(b => b.Provider)
                    .ToListAsync();
            }
        }

        public async Task<Film> UpselectFilm(string filmName, DateTimeOffset projectionScehdule)
        {
            using (var db = dbContextFactory.Create())
            {
                var film = await db.Films
                    .FirstOrDefaultAsync(x => x.FilmName == filmName);
                if (film == null)
                {
                    film = new Film()
                    {
                        FilmName = filmName,
                        FirstProjectionDate = projectionScehdule.UtcDateTime
                    };
                    await db.Films
                        .AddAsync(film);
                    await db.SaveChangesAsync();
                    return film;
                }
                if(film.FirstProjectionDate < projectionScehdule)
                {
                    db.Update(film);
                    film.FirstProjectionDate = projectionScehdule.UtcDateTime;
                    await db.SaveChangesAsync();                    
                }
                return film;
            }
        }

        public async Task LoadSample(long projectionsScheduleId, long boughtSeats, long lockedSeats, long reservedSeats, long quarantinedSeats, long totalSeats)
        {
            using(var db = dbContextFactory.Create())
            {
                await db.Samples.AddAsync(new Sample()
                {
                    SampleTimestamp = DateTime.UtcNow,
                    ProjectionsScheduleId = projectionsScheduleId,
                    BoughtSeats = boughtSeats,
                    LockedSeats = lockedSeats,
                    ReservedSeats = reservedSeats,
                    QuarantinedSeats = quarantinedSeats,
                    TotalSeats = totalSeats
                });
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProjectionsSchedule>> ListProjectionsScheduleAsync(TimeSpan timeSpan)
        {
            using (var db = dbContextFactory.Create())
            {
                return await db.ProjectionsSchedules.Where(x => x.ProjectionTimestamp > DateTime.UtcNow && x.ProjectionTimestamp < DateTime.UtcNow.Add(timeSpan)).ToListAsync();
            }
        }
    }
}
