using Cadorinator.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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

        private readonly string _connectionString;
        private readonly string _mainDbName;

        public MainContext() { }

        public MainContext(string conn, string mainDbName)
        {
            Directory.CreateDirectory(conn);
            _mainDbName = mainDbName;
            _connectionString = conn;
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder.UseSqlite("Data Source=" + _connectionString + _mainDbName + ";");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
