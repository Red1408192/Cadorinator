using Cadorinator.Infrastructure.Entity;
using Cadorinator.Infrastructure.Interface;
using Cadorinator.Model;
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

        public CadorinatorService(ICadorinatorSettings settings, IProviderService[] providerServices, IDALService dALService)
        {
            _settings = settings;
            _providerServices = providerServices;
            _dalService = dALService;
        }

        public async Task<IList<Operation>> CollectSchedulesAsync()
        {
            foreach (var provider in await _dalService.ListProvidersAsync())
            {
                await _providerServices.First(x => x.ProviderSourceId == provider.ProviderSource).RegisterSchedules(provider);
                await Task.Delay(_settings.DefaultDelay);
            }

            return new List<Operation>()
            {
                new Operation()
                {
                    Function = () => this.CollectSchedulesAsync(),
                    ActionType = ActionType.CollectSchedules,
                    Identifier = "CS",
                    ScheduledTime = DateTime.UtcNow.AddHours(_settings.PollerTimeSpan),
                    RequireCoolDown = false
                }
            };
        }

        public async Task<IList<Operation>> CheckSchedulesAsync(TimeSpan timeSpan, List<Operation> operations)
        {
            operations.Add(new Operation()
            {
                ActionType = ActionType.CheckSchedules,
                Function = () => CheckSchedulesAsync(timeSpan, operations),
                Identifier = "CHS",
                RequireCoolDown = false,
                ScheduledTime = DateTime.UtcNow.AddHours(_settings.SchedulerTimeSpan)
            });

            foreach (var schedule in await _dalService.ListProjectionsScheduleAsync(timeSpan))
            {
                foreach (var timeOffsetToCollect in _settings.SamplesRange)
                {
                    var id = $"T:{schedule.ProjectionsScheduleId}O:-{timeOffsetToCollect}";
                    if (!operations.Any(x => x.Identifier == id) && DateTime.UtcNow < schedule.ProjectionTimestamp.AddSeconds(-timeOffsetToCollect))
                    {
                        operations.Add(new Operation()
                        {
                            ActionType = ActionType.TakeSample,
                            Function = () => this.TakeSample(schedule, timeOffsetToCollect),
                            ScheduledTime = schedule.ProjectionTimestamp.AddSeconds(-timeOffsetToCollect),
                            Identifier = id,
                            RequireCoolDown = true
                        });
                    }
                }
            }
            return operations;
        }

        public async Task<IList<Operation>> TakeSample(ProjectionsSchedule schedule, int eta)
        {
            await _providerServices.First(x => x.ProviderSourceId == schedule.Provider.ProviderSource).SampleData(schedule, eta);
            return new List<Operation>();
        }
    }
}
