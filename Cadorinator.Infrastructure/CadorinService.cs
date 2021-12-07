using Cadorinator.Infrastructure.Entity;
using Cadorinator.ServiceContract.Settings;
using System;
using System.Linq;

namespace Cadorinator.Infrastructure
{
    public class CadorinService
    {
        private readonly IDbContextFactory<MainContext> dbContextFactory;

        public CadorinService(IDbContextFactory<MainContext> dbContext, ICadorinatorSettings settings)
        {
            this.dbContextFactory = dbContext;
        }

        public void Test()
        {
            using(var db = dbContextFactory.Create())
            {
                db.Films.Add(new Film() { FilmName = "Test", FirstProjectionDate = DateTime.Now });

                db.SaveChanges();

                var sel = db.Films.ToList();
            }
        }
    }
}
