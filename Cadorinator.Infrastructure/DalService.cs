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
        Task<bool> AddSchedule(ProjectionsSchedule schedule);
        Task<IEnumerable<Provider>> ListProvidersAsync(long? providerSource = null);
        Task<bool> LoadSample(long projectionsScheduleId, long boughtSeats, long lockedSeats, long reservedSeats, long quarantinedSeats, long totalSeats, int secondsETA);
        Task<Film> UpselectFilm(string filmName, DateTimeOffset projectionScehdule);
        Task<IEnumerable<ProjectionsSchedule>> ListProjectionsScheduleAsync(TimeSpan timeSpan);
        Task<Theater> UpselectTheater(string theaterName, long ProviderId);
    }

    public class DalService : IDALService
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<MainContext> dbContextFactory;

        public DalService(IDbContextFactory<MainContext> dbContext, ICadorinatorSettings settings, ILogger logger)
        {
            _logger = logger;
            dbContextFactory = dbContext;
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

        public async Task<bool> AddSchedule(ProjectionsSchedule schedule)
        {
            try
            {
                using (var db = dbContextFactory.Create())
                {
                    if (await db.ProjectionsSchedules.AnyAsync(x => x.FilmId == schedule.FilmId 
                                                                && x.ProjectionTimestamp == schedule.ProjectionTimestamp
                                                                && x.ProviderId == x.ProviderId
                                                                && x.ThreaterId == schedule.ThreaterId)) return false;
                    await db.ProjectionsSchedules
                        .AddAsync(schedule);
                    var res = await db.SaveChangesAsync();
                    return res > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Add schedule");
                return false;
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
                        _logger.Information($"{filmName} added as a film");
                        return film;
                    }
                    if (film.FirstProjectionDate > projectionScehdule)
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

        public async Task<Theater> UpselectTheater(string theaterName, long ProviderId)
        {
            try
            {
                using (var db = dbContextFactory.Create())
                {
                    var theater = await db.Theaters
                        .FirstOrDefaultAsync(x => x.TheaterName == theaterName);
                    if (theater == null)
                    {
                        theater = new Theater()
                        {
                            TheaterName = theaterName,
                            ProviderId = ProviderId
                        };
                        await db.Theaters
                            .AddAsync(theater);
                        await db.SaveChangesAsync();
                        _logger.Information($"{theaterName} added as theater");
                        return theater;
                    }
                    return theater;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Upselect Theater");
                return default;
            }
        }

        public async Task<bool> LoadSample(long projectionsScheduleId, long boughtSeats, long lockedSeats, long reservedSeats, long quarantinedSeats, long totalSeats, int secondsETA)
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
                    var res = await db.SaveChangesAsync();
                    _logger.Information($"sample collected");
                    return res > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Load Sample");
                return false;
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
