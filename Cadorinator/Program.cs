using Cadorinator.Infrastructure;
using Cadorinator.Infrastructure.Entity;
using Cadorinator.Infrastructure.Interface;
using Cadorinator.Model;
using Cadorinator.Service.Model;
using Cadorinator.Service.Service;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadorinator.Service
{
    partial class Program
    {
        private static ICadorinatorSettings _settings;
        private static IDbContextFactory<MainContext> _dbContextFactory;
        private static IDALService _dalService;
        private static ICadorinatorService _service;
        private static IProviderService[] _providerServices;
        private static OperationSchedule Operations;

        static async Task Main(string[] args)
        {
            await SetupAsync();
            if (!Operations.Any(ActionType.CollectSchedules)) await _service.CollectSchedulesAsync(Operations);
            if (!Operations.Any(ActionType.CheckSchedules)) await _service.CheckSchedulesAsync(new TimeSpan(_settings.PollerTimeSpan, 0, 0), Operations);
            while (true)
            {
                foreach(var op in Operations.GetOperations(DateTime.UtcNow))
                {
                    if(await op.Value.Function(Operations)) Operations.Pop(op.Key);
                }
                await Task.Delay(_settings.Reactivity);
            }
        }

        private static async Task SetupAsync()
        {
            _settings = await Settings.LoadAsync();
            _dbContextFactory = new DbContextFactory(_settings.FilePath, _settings.MainDbName);
            _dalService = new DalService(_dbContextFactory, _settings);
            Operations = new OperationSchedule(_settings.MaxRequestOffset);

            _providerServices = new IProviderService[]
            {
                new EighteenTicketV1Service(_dalService, _settings),
                new EighteenTicketV2Service(_dalService, _settings)
            };
            _service = new CadorinatorService(_settings, _providerServices, _dalService);
        }
    }
}
