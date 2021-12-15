using Cadorinator.Infrastructure;
using Cadorinator.Infrastructure.Entity;
using Cadorinator.Infrastructure.Interface;
using Cadorinator.Model;
using Cadorinator.Service.Model;
using Cadorinator.Service.Service;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static ILogger _logger;

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
            var startUpLogger = new LoggerConfiguration().WriteTo.File("./log.txt").CreateLogger();
            startUpLogger.Warning("STARTUP CADORINATOR");
            _settings = await Settings.LoadAsync(startUpLogger);
            _logger = new LoggerConfiguration()
                .WriteTo.File(path: _settings.FilePath + "//log.txt",
                restrictedToMinimumLevel: (LogEventLevel)_settings.LoggingLevel)
                .CreateLogger();
            _logger.Warning("Startup");
            _dbContextFactory = new DbContextFactory(_settings, _logger);
            _dalService = new DalService(_dbContextFactory, _settings, _logger);
            Operations = new OperationSchedule(_settings.MaxRequestOffset, _logger);

            _providerServices = new IProviderService[]
            {
                new EighteenTicketV1Service(_dalService, _settings, _logger),
                new EighteenTicketV2Service(_dalService, _settings, _logger)
            };
            _service = new CadorinatorService(_settings, _providerServices, _dalService, _logger);
            startUpLogger.Warning("setup completed");
        }
    }
}
