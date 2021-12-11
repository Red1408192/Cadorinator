using Cadorinator.Infrastructure;
using Cadorinator.Infrastructure.Entity;
using Cadorinator.Model;
using Cadorinator.Service.Service;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
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
        private static IProviderService[] _providerServices;
        private static List<Operation> Operations = new List<Operation>();

        static async Task Main(string[] args)
        {
            await SetupAsync();
            while (true)
            {
                if(Operations.Any(x => x.ActionType == ActionType.CollectSchedules))
                {
                }
            }
        }

        private static async Task SetupAsync()
        {
            _settings = await Settings.LoadAsync();
            _dbContextFactory = new DbContextFactory(_settings.FilePath, _settings.MainDbName);
            _dalService = new DalService(_dbContextFactory, _settings);

            _providerServices = new IProviderService[]
            {
                new EighteenTicketV1Service(_dalService, _settings),
                new EighteenTicketV2Service(_dalService)
            };
        }
    }
}
