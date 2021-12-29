using Cadorinator.Infrastructure.Entity;
using Cadorinator.Infrastructure.Interface;
using Cadorinator.Model;
using Cadorinator.Service.Model;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadorinator.Infrastructure
{
    public class CadorinatorService : ICadorinatorService
    {
        private readonly ICadorinatorSettings _settings;
        private readonly IProviderService[] _providerServices;
        private readonly IDALService _dalService;
        private ILogger _logger;
        private long _lastSuccessfullProviderId;
        private bool _startUpMode;

        public CadorinatorService(ICadorinatorSettings settings, IProviderService[] providerServices, IDALService dALService, ILogger logger)
        {
            _settings = settings;
            _providerServices = providerServices;
            _dalService = dALService; 
            _startUpMode = true;
            _logger = logger;
        }

        public async Task<bool> CollectSchedulesAsync(OperationSchedule operationSchedule, long? providerId = null)
        {
            try
            {
                providerId = 27;
                _logger.Information(_startUpMode ? "Collecting Schedule in startup mode" : "Collecting Schedule");
                var providers = await _dalService.ListProvidersAsync();
                var idIndex = providers.Select(x => x.ProviderId).OrderBy(x => x);

                if (providerId is null) providerId = idIndex.First();
                if (idIndex.Last() < providerId)
                {
                    providerId = idIndex.First();
                    _logger.Information(_startUpMode ? "Ending Startup mode" : "Completed schedule collecting");
                    _startUpMode = false;
                }

                var provider = providers.FirstOrDefault(x => x.ProviderId == providerId);
                bool res = false;
                if (provider is not null)
                {
                    res = await _providerServices.First(x => x.ProviderSourceId == provider.ProviderSource).RegisterSchedules(provider);
                    if (!res) return false;
                    _lastSuccessfullProviderId = providerId.Value;
                }
                _logger.Information($"Done, next collection at {DateTime.UtcNow.AddMinutes(_startUpMode ? 1 : _settings.PollerTimeSpan)}");

                return operationSchedule.AddOperation(new Operation()
                {
                    Function = (x) => this.CollectSchedulesAsync(x, providerId+1),
                    ActionType = ActionType.CollectSchedules,
                    Identifier = "CS",
                    ProspectedTime = DateTime.UtcNow.AddMinutes(_startUpMode ? 1 : _settings.PollerTimeSpan)
                }, required: true);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error during collection");
                return operationSchedule.AddOperation(new Operation()
                {
                    Function = (x) => this.CollectSchedulesAsync(x, providerId+1),
                    ActionType = ActionType.CollectSchedules,
                    Identifier = "CS",
                    ProspectedTime = DateTime.UtcNow.AddMinutes(_startUpMode ? 1 : _settings.PollerTimeSpan)
                }, required: true);
            }
        }

        public async Task<bool> CheckSchedulesAsync(TimeSpan timeSpan, OperationSchedule operationSchedule)
        {
            try
            {
                operationSchedule.AddOperation(new Operation()
                {
                    ActionType = ActionType.CheckSchedules,
                    Function = (x) => CheckSchedulesAsync(timeSpan, x),
                    Identifier = "CHS",
                    ProspectedTime = DateTime.UtcNow.AddMinutes(_startUpMode ? 1 : _settings.SchedulerTimeSpan)
                }, required: true);
                int count = 0;
                foreach (var schedule in await _dalService.ListProjectionsScheduleAsync(timeSpan))
                {
                    foreach (var timeOffsetToCollect in _settings.SamplesRange)
                    {
                        var id = $"T:{schedule.ProjectionsScheduleId}O:{-timeOffsetToCollect}";
                        var timeSchedule = schedule.ProjectionTimestamp.ToUniversalTime().AddSeconds(-timeOffsetToCollect);
                        if (DateTime.UtcNow > timeSchedule) continue;

                        if (!operationSchedule.Any(id))
                        {
                            operationSchedule.AddOperation(new Operation()
                            {
                                ActionType = ActionType.TakeSample,
                                Function = (x) => this.TakeSample(schedule, timeOffsetToCollect),
                                ProspectedTime = timeSchedule,
                                Identifier = id
                            });
                            count++;
                        }
                    }
                }
                _logger.Information($"Completed Scehduleding - {count} operations added - Next:{(DateTime.UtcNow.AddMinutes(_startUpMode ? 1 : _settings.SchedulerTimeSpan))}");
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Exception during scheduling");
                return false;
            }
        }

        public async Task<bool> TakeSample(ProjectionsSchedule schedule, int eta)
        {
            return await _providerServices.First(x => x.ProviderSourceId == schedule.Provider.ProviderSource).SampleData(schedule, eta);
        }
    }
}
