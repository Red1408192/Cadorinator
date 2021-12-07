using Cadorinator.Infrastructure;
using System.Threading.Tasks;

namespace Cadorinator.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var st = await Settings.LoadAsync();
            var contextFactory = await Task.Run(() => new DbContextFactory(st.DbPath, st.MainDbName));

            new CadorinService(contextFactory, st).Test();
        }
    }
}
