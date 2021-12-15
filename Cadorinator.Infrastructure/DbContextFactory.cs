using Cadorinator.ServiceContract.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cadorinator.Infrastructure
{
    public class DbContextFactory : IDbContextFactory<MainContext>
    {
        private readonly ICadorinatorSettings _settings;
        private readonly ILogger _logger;

        public DbContextFactory(ICadorinatorSettings settings, ILogger logger)
        {
            _settings = settings;
            _logger = logger;
            Setup();
        }

        public MainContext Create()
        {
            return new MainContext(_settings, _logger);
        }

        public void Setup()
        {
            new MainContext(_settings, _logger).Setup();
        }
    }

    public interface IDbContextFactory<TContext> where TContext : DbContext
    {
        TContext Create();
    }
}
