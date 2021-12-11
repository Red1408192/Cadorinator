using Cadorinator.Model;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadorinator.Infrastructure
{
    public class CadorinatorService
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
            foreach(var provider in await _dalService.ListProvidersAsync())
            {
                await _providerServices.First(x => x.ProviderSourceId == provider.ProviderSource).RegisterSchedules(provider);
                await Task.Delay(_settings.DefaultDelay);
            }

            return new List<Operation>()
            {
                new Operation()
                {
                    Function = this.CollectSchedulesAsync,
                    ActionType = ActionType.CollectSchedules,
                    Identifier = "CS",
                    ScheduledTime = DateTimeOffset.Now.AddHours(2)
                }
            };
        }

        public async Task<IList<Operation>> CheckSchedulesAsync()
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
                    Function = this.CollectSchedulesAsync,
                    ActionType = ActionType.CollectSchedules,
                    Identifier = "CS",
                    ScheduledTime = DateTimeOffset.Now.AddHours(2)
                }
            };
        }
    }
}
