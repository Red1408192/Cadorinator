using Cadorinator.Infrastructure.Entity;
using Cadorinator.ServiceContract.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadorinator.Infrastructure
{
    public interface ICadorinatorService
    {
        Task AddSchedule(ProjectionsSchedule schedule);
        Task<IEnumerable<Provider>> ListProvidersAsync(long providerSource);
        Task LoadSample(long projectionsScheduleId, long boughtSeats, long lockedSeats, long reservedSeats, long quarantinedSeats, long totalSeats);
        Task<Film> UpselectFilm(string filmName, DateTime projectionScehdule);
    }

    public class CadorinatorService : ICadorinatorService
    {
        private readonly IDbContextFactory<MainContext> dbContextFactory;

        public CadorinatorService(IDbContextFactory<MainContext> dbContext, ICadorinatorSettings settings)
        {
            this.dbContextFactory = dbContext;
        }

        public async Task<IEnumerable<Provider>> ListProvidersAsync(long providerSource)
        {
            using (var db = dbContextFactory.Create())
            {
                return await db.Providers.Where(x => x.ProviderSource == providerSource).ToArrayAsync();
            }
        }

        public async Task AddSchedule(ProjectionsSchedule schedule)
        {
            using (var db = dbContextFactory.Create())
            {
                await db.ProjectionsSchedules.AddAsync(schedule);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Film> UpselectFilm(string filmName, DateTime projectionScehdule)
        {
            using (var db = dbContextFactory.Create())
            {
                var film = await db.Films.FirstOrDefaultAsync(x => x.FilmName == filmName);
                if (film == null)
                {
                    film = new Film()
                    {
                        FilmName = filmName,
                        FirstProjectionDate = projectionScehdule
                    };
                    await db.Films.AddAsync(film);
                    await db.SaveChangesAsync();
                    return film;
                }
                if(film.FirstProjectionDate < projectionScehdule)
                {
                    db.Update(film);
                    film.FirstProjectionDate = projectionScehdule;
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
                    SampleTimestamp = DateTime.Now,
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
    }
}
