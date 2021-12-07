using Microsoft.EntityFrameworkCore;

namespace Cadorinator.Infrastructure
{
    public class DbContextFactory : IDbContextFactory<MainContext>
    {
        private readonly string connectionString;
        private readonly string dbName;

        public DbContextFactory(string conn, string name)
        {
            connectionString = conn;
            dbName = name;
        }

        public MainContext Create()
        {
            return new MainContext(connectionString, dbName);
        }
    }

    public interface IDbContextFactory<TContext> where TContext : DbContext
    {
        TContext Create();
    }
}
