using Cadorinator.Infrastructure;
using Cadorinator.Infrastructure.Entity;
using Cadorinator.Service.Service;
using Cadorinator.ServiceContract.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace Cadorinator.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var st = await Settings.LoadAsync();
            var contextFactory = await Task.Run(() => new DbContextFactory(st.FilePath, st.MainDbName));
            var sched = contextFactory.Create().ProjectionsSchedules.First(x => x.ProjectionsScheduleId == 2);
            sched.Film = contextFactory.Create().Films.First(x => x.FilmId == sched.FilmId);
            sched.Provider = contextFactory.Create().Providers.First(x => x.ProviderId == sched.ProviderId);
            await new EighteenTicketV2Service(new CadorinatorService(contextFactory, st)).SampleData(sched);
        }
    }
}
