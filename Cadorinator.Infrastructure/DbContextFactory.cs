using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cadorinator.Infrastructure
{
    public class DbContextFactory : IDbContextFactory<MainContext>
    {
        private readonly string connectionString;
        private readonly string dbName;
        private readonly ILogger _logger;

        public DbContextFactory(string conn, string name, ILogger logger)
        {
            connectionString = conn;
            dbName = name;
            _logger = logger;
        }

        public MainContext Create()
        {
            return new MainContext(connectionString, dbName, _logger);
        }
    }

    public interface IDbContextFactory<TContext> where TContext : DbContext
    {
        TContext Create();
    }
}
