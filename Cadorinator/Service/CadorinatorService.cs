using Cadorinator.Infrastructure.Entity;
using Cadorinator.Infrastructure.Interface;
using Cadorinator.Model;
using Cadorinator.Service.Model;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
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
        private long _lastSuccessfullProviderId;
        private bool _startUpMode;

        public CadorinatorService(ICadorinatorSettings settings, IProviderService[] providerServices, IDALService dALService)
        {
            _settings = settings;
            _providerServices = providerServices;
            _dalService = dALService; 
            _startUpMode = true;
        }

        public async Task<bool> CollectSchedulesAsync(OperationSchedule operationSchedule, long? providerId = null)
        {
            var providers = await _dalService.ListProvidersAsync();
            var idIndex = providers.Select(x => x.ProviderId).OrderBy(x => x);

            if (providerId is null) providerId = idIndex.First();
            if (idIndex.Last() < providerId)
            {
                providerId = idIndex.First();
                _startUpMode = false;
            }

            var provider = providers.FirstOrDefault(x => x.ProviderId == providerId);
            if (provider is not null)
            {
                await _providerServices.First(x => x.ProviderSourceId == provider.ProviderSource).RegisterSchedules(provider);
                _lastSuccessfullProviderId = providerId.Value;
            }

            return operationSchedule.AddOperation(new Operation()
            {
                Function = (x) => this.CollectSchedulesAsync(x, providerId++),
                ActionType = ActionType.CollectSchedules,
                Identifier = "CS",
                ProspectedTime = DateTime.UtcNow.AddMinutes(_startUpMode? 5 : _settings.PollerTimeSpan)
            }, required: true);
        }

        public async Task<bool> CheckSchedulesAsync(TimeSpan timeSpan, OperationSchedule operationSchedule)
        {
            operationSchedule.AddOperation(new Operation()
            {
                ActionType = ActionType.CheckSchedules,
                Function = (x) => CheckSchedulesAsync(timeSpan, x),
                Identifier = "CHS",
                ProspectedTime = DateTime.UtcNow.AddMinutes(_startUpMode? 5 : _settings.SchedulerTimeSpan)
            }, required: true);

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
                    }
                }
            }
            return true;
        }

        public async Task<bool> TakeSample(ProjectionsSchedule schedule, int eta)
        {
            await _providerServices.First(x => x.ProviderSourceId == schedule.Provider.ProviderSource).SampleData(schedule, eta);
            return true;
        }
    }
}
