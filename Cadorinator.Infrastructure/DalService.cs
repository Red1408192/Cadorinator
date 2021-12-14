using Cadorinator.Infrastructure.Entity;
using Cadorinator.ServiceContract.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
        Task LoadSample(long projectionsScheduleId, long boughtSeats, long lockedSeats, long reservedSeats, long quarantinedSeats, long totalSeats, int secondsETA);
        Task<Film> UpselectFilm(string filmName, DateTimeOffset projectionScehdule);
        Task<IEnumerable<ProjectionsSchedule>> ListProjectionsScheduleAsync(TimeSpan timeSpan);
    }

    public class DalService : IDALService
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<MainContext> dbContextFactory;

        public DalService(IDbContextFactory<MainContext> dbContext, ICadorinatorSettings settings, ILogger logger)
        {
            this._logger = logger;
            this.dbContextFactory = dbContext;
        }

        public async Task<IEnumerable<Provider>> ListProvidersAsync(long? providerSource = null)
        {
            try
            {
                using (var db = dbContextFactory.Create())
                {
                    return await db.Providers
                        .Where(x => x.ProviderSource == (providerSource ?? x.ProviderSource))
                        .ToListAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Exception during ListProvider");
                return default;
            }
        }

        public async Task AddSchedule(ProjectionsSchedule schedule)
        {
            try
            {
                using (var db = dbContextFactory.Create())
                {
                    if (await db.ProjectionsSchedules.AnyAsync(x => x.FilmId == schedule.FilmId && x.ProjectionTimestamp == schedule.ProjectionTimestamp && x.ProviderId == x.ProviderId)) return;
                    await db.ProjectionsSchedules
                        .AddAsync(schedule);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Add schedule");
            }
        }

        public async Task<Film> UpselectFilm(string filmName, DateTimeOffset projectionScehdule)
        {
            try
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
                    if (film.FirstProjectionDate < projectionScehdule)
                    {
                        db.Update(film);
                        film.FirstProjectionDate = projectionScehdule.UtcDateTime;
                        await db.SaveChangesAsync();
                    }
                    return film;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Upselect Film");
                return default;
            }
        }

        public async Task LoadSample(long projectionsScheduleId, long boughtSeats, long lockedSeats, long reservedSeats, long quarantinedSeats, long totalSeats, int secondsETA)
        {
            try
            {
                using (var db = dbContextFactory.Create())
                {
                    await db.Samples.AddAsync(new Sample()
                    {
                        SampleTimestamp = DateTime.UtcNow,
                        ETA = FormatHelper.FormatEta(secondsETA),
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
            catch (Exception ex)
            {
                _logger.Error(ex, $"Load Sample");
            }
        }

        public async Task<IEnumerable<ProjectionsSchedule>> ListProjectionsScheduleAsync(TimeSpan timeSpan)
        {
            try
            {
                var From = DateTime.UtcNow;
                var To = DateTime.UtcNow.Add(timeSpan);

                using (var db = dbContextFactory.Create())
                {
                    return await db.ProjectionsSchedules
                        .Where(x => x.ProjectionTimestamp > From && x.ProjectionTimestamp < To)
                        .Include(x => x.Provider)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"List Projections Schedule");
                return default;
            }
        }
    }
}
