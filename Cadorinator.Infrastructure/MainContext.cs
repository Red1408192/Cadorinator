using Cadorinator.Infrastructure.Entity;
using Cadorinator.ServiceContract.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cadorinator.Infrastructure
{
    public partial class MainContext : DbContext
    {
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<PendingCommand> PendingCommands { get; set; }
        public virtual DbSet<ProjectionsSchedule> ProjectionsSchedules { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<ProviderSource> ProviderSources { get; set; }
        public virtual DbSet<Sample> Samples { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Theater> Theaters { get; set; }

        private readonly ICadorinatorSettings _settings;
        private readonly ILogger _logger;

        public MainContext() { }

        public MainContext(ICadorinatorSettings settings, ILogger logger)
        {
            try
            {
                _settings = settings;
                _logger = logger;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"{DateTime.Now} [Ex] - exception during context creation {ex.Message}");
            }
        }

        internal void Setup()
        {
            Directory.CreateDirectory(_settings.FilePath);
            this.Database.Migrate();
            this.Database.ExecuteSqlRaw("DROP VIEW IF EXISTS SampleView;\n");
            this.Database.ExecuteSqlRaw(CreateDynamicView(_settings.SamplesRange));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder.UseSqlite("Data Source=" + _settings.FilePath + _settings.MainDbName + ";");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("Film");

                entity.HasIndex(e => e.FilmId, "IX_Film_FilmId")
                    .IsUnique();

                entity.Property(e => e.FilmName).IsRequired();

                entity.Property(e => e.FirstProjectionDate).IsRequired();
            });

            modelBuilder.Entity<PendingCommand>(entity =>
            {
                entity.ToTable("PendingCommand");

                entity.HasIndex(e => e.PendingCommandId, "IX_PendingCommand_PendingCommandId")
                    .IsUnique();

                entity.Property(e => e.CommandId).HasColumnType("TINYINT");

                entity.Property(e => e.TimeStamp)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasColumnType("Parameters");
            });

            modelBuilder.Entity<ProjectionsSchedule>(entity =>
            {
                entity.ToTable("ProjectionsSchedule");

                entity.HasIndex(e => e.ProjectionsScheduleId, "IX_ProjectionsSchedule_ProjectionsScheduleId")
                    .IsUnique();

                entity.Property(e => e.ProjectionTimestamp)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.SourceEndpoint)
                    .IsRequired()
                    .HasColumnType("VARCHAR(100)");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.ProjectionsSchedules)
                    .HasForeignKey(d => d.FilmId);

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ProjectionsSchedules)
                    .HasForeignKey(d => d.ProviderId);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("Provider");

                entity.HasIndex(e => e.ProviderId, "IX_Provider_ProviderId")
                    .IsUnique();

                entity.Property(e => e.ProviderDomain)
                    .IsRequired()
                    .HasColumnType("VARCHAR(60)");

                entity.HasOne(d => d.ProviderSourceNavigation)
                    .WithMany(p => p.Providers)
                    .HasForeignKey(d => d.ProviderSource);

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.Providers)
                    .HasForeignKey(d => d.CityId);
            });

            modelBuilder.Entity<ProviderSource>(entity =>
            {
                entity.ToTable("ProviderSource");

                entity.HasIndex(e => e.ProviderSourceId, "IX_ProviderSource_ProviderSourceId")
                    .IsUnique();

                entity.Property(e => e.ProviderSourceName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(30)");
            });

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.ToTable("Sample");

                entity.HasIndex(e => e.SampleId, "IX_Sample_SampleId")
                    .IsUnique();

                entity.Property(e => e.BoughtSeats).HasColumnType("SMALLINT");

                entity.Property(e => e.LockedSeats).HasColumnType("SMALLINT");

                entity.Property(e => e.QuarantinedSeats).HasColumnType("SMALLINT");

                entity.Property(e => e.ReservedSeats).HasColumnType("SMALLINT");

                entity.Property(e => e.SampleTimestamp)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.TotalSeats).HasColumnType("SMALLINT");

                entity.HasOne(d => d.ProjectionsSchedule)
                    .WithMany(p => p.Samples)
                    .HasForeignKey(d => d.ProjectionsScheduleId);
            });

            modelBuilder.Entity<Theater>(entity =>
            {
                entity.ToTable("Theater");

                entity.HasIndex(e => e.TheaterId, "IX_Theater_TheaterId")
                    .IsUnique();

                entity.Property(e => e.TheaterName).HasColumnType("VARCHAR(10)");
                entity.HasOne(e => e.ProviderNavigation)
                    .WithMany(e => e.Theaters)
                    .HasForeignKey(e => e.ProviderId);

            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.HasIndex(e => e.CityId, "IX_City_CityId")
                    .IsUnique();

                entity.Property(e => e.CityName)
                .IsRequired()
                .HasColumnType("VARCHAR(30)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private string CreateDynamicView(int[] sampleRanges)
        {
            var query = "";
            query += "CREATE VIEW SampleView\n";
            query += "(Film\n,Cinema\n,City\n,Theater\n,Date\n,Time\n";
            query += $",'Total Seats'\n";
            query += sampleRanges
                        .OrderBy(x => x)
                        .Aggregate("", (x, y) => x +
                         $",'Bought({FormatHelper.FormatEta(y)})'\n" +
                         $",'Locked({FormatHelper.FormatEta(y)})'\n" +
                         $",'Quarantined({FormatHelper.FormatEta(y)})'\n") + ")\n";
            query += "AS\n";
            query += "SELECT\n";
            query += "F.FilmName AS Film\n";
            query += ",P.ProviderDomain AS Provider\n";
            query += ",C.CityName AS City\n";
            query += ",T.TheaterName AS Theater\n";
            query += ",Date(PS.ProjectionTimestamp, 'localtime')   AS Date\n";
            query += ",Time(PS.ProjectionTimestamp, 'localtime')   AS Time\n";
            query += $",Sp{ Sql(sampleRanges.OrderBy(x => x).First())}.TotalSeats AS 'Total'\n";
            query += sampleRanges
                        .OrderBy(x => x)
                        .Aggregate("", (x, y) => x +
                         $",Sp{Sql(y)}.BoughtSeats AS 'Bought({FormatHelper.FormatEta(y)})'\n" +
                         $",Sp{Sql(y)}.LockedSeats AS 'Locked({FormatHelper.FormatEta(y)})'\n" +
                         $",Sp{Sql(y)}.QuarantinedSeats AS 'Quarantined({FormatHelper.FormatEta(y)})'\n");
            query += "FROM ProjectionsSchedule PS\n";
            query += sampleRanges
                        .OrderBy(x => x)
                        .Take(1)
                        .Select(y => $"JOIN Sample SP{Sql(y)} ON PS.ProjectionsScheduleId = SP{Sql(y)}.ProjectionsScheduleId AND SP{Sql(y)}.Eta = '{FormatHelper.FormatEta(y)}'\n").First();
            query += sampleRanges
                        .OrderBy(x => x)
                        .Skip(1)
                        .Aggregate("", (x, y) => x + $"LEFT JOIN Sample SP{Sql(y)} ON PS.ProjectionsScheduleId = SP{Sql(y)}.ProjectionsScheduleId AND SP{Sql(y)}.Eta = '{FormatHelper.FormatEta(y)}'\n");
            query += "JOIN Theater T ON T.TheaterId = PS.ThreaterId\n";
            query += "JOIN City C ON C.CityId = P.CityId\n";
            query += "JOIN Provider P ON P.ProviderId = PS.ProviderId\n";
            query += "JOIN Film F ON F.FilmId = PS.FilmId\n";
            return query;
        }
        private string Sql(int i) => i.ToString().Replace("-", "m");
    }
}
