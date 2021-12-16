﻿// <auto-generated />
using System;
using Cadorinator.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cadorinator.Infrastructure.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20211215160203_TheaterTable")]
    partial class TheaterTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.City", b =>
                {
                    b.Property<long>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(30)");

                    b.HasKey("CityId");

                    b.HasIndex(new[] { "CityId" }, "IX_City_CityId")
                        .IsUnique();

                    b.ToTable("City");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Film", b =>
                {
                    b.Property<long>("FilmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FilmName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FirstProjectionDate")
                        .HasColumnType("TEXT");

                    b.HasKey("FilmId");

                    b.HasIndex(new[] { "FilmId" }, "IX_Film_FilmId")
                        .IsUnique();

                    b.ToTable("Film");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.PendingCommand", b =>
                {
                    b.Property<long>("PendingCommandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CommandId")
                        .HasColumnType("TINYINT");

                    b.Property<string>("Parameters")
                        .IsRequired()
                        .HasColumnType("Parameters");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("DATETIME");

                    b.HasKey("PendingCommandId");

                    b.HasIndex(new[] { "PendingCommandId" }, "IX_PendingCommand_PendingCommandId")
                        .IsUnique();

                    b.ToTable("PendingCommand");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.ProjectionsSchedule", b =>
                {
                    b.Property<long>("ProjectionsScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("FilmId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ProjectionTimestamp")
                        .HasColumnType("DATETIME");

                    b.Property<long>("ProviderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceEndpoint")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<long>("ThreaterId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProjectionsScheduleId");

                    b.HasIndex("FilmId");

                    b.HasIndex("ProviderId");

                    b.HasIndex(new[] { "ProjectionsScheduleId" }, "IX_ProjectionsSchedule_ProjectionsScheduleId")
                        .IsUnique();

                    b.ToTable("ProjectionsSchedule");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Provider", b =>
                {
                    b.Property<long>("ProviderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProviderDomain")
                        .IsRequired()
                        .HasColumnType("VARCHAR(60)");

                    b.Property<string>("ProviderName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<long>("ProviderSource")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProviderId");

                    b.HasIndex("CityId");

                    b.HasIndex("ProviderSource");

                    b.HasIndex(new[] { "ProviderId" }, "IX_Provider_ProviderId")
                        .IsUnique();

                    b.ToTable("Provider");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.ProviderSource", b =>
                {
                    b.Property<long>("ProviderSourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProviderSourceName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(30)");

                    b.HasKey("ProviderSourceId");

                    b.HasIndex(new[] { "ProviderSourceId" }, "IX_ProviderSource_ProviderSourceId")
                        .IsUnique();

                    b.ToTable("ProviderSource");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Sample", b =>
                {
                    b.Property<long>("SampleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("BoughtSeats")
                        .HasColumnType("SMALLINT");

                    b.Property<string>("ETA")
                        .HasColumnType("TEXT");

                    b.Property<long>("LockedSeats")
                        .HasColumnType("SMALLINT");

                    b.Property<long>("ProjectionsScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("QuarantinedSeats")
                        .HasColumnType("SMALLINT");

                    b.Property<long>("ReservedSeats")
                        .HasColumnType("SMALLINT");

                    b.Property<DateTime>("SampleTimestamp")
                        .HasColumnType("DATETIME");

                    b.Property<long>("TotalSeats")
                        .HasColumnType("SMALLINT");

                    b.HasKey("SampleId");

                    b.HasIndex("ProjectionsScheduleId");

                    b.HasIndex(new[] { "SampleId" }, "IX_Sample_SampleId")
                        .IsUnique();

                    b.ToTable("Sample");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Theater", b =>
                {
                    b.Property<long>("TheaterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ProviderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TheaterName")
                        .HasColumnType("VARCHAR(10)");

                    b.HasKey("TheaterId");

                    b.HasIndex("ProviderId");

                    b.HasIndex(new[] { "TheaterId" }, "IX_Theater_TheaterId")
                        .IsUnique();

                    b.ToTable("Theater");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.ProjectionsSchedule", b =>
                {
                    b.HasOne("Cadorinator.Infrastructure.Entity.Film", "Film")
                        .WithMany("ProjectionsSchedules")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cadorinator.Infrastructure.Entity.Provider", "Provider")
                        .WithMany("ProjectionsSchedules")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Provider", b =>
                {
                    b.HasOne("Cadorinator.Infrastructure.Entity.City", "CityNavigation")
                        .WithMany("Providers")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cadorinator.Infrastructure.Entity.ProviderSource", "ProviderSourceNavigation")
                        .WithMany("Providers")
                        .HasForeignKey("ProviderSource")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CityNavigation");

                    b.Navigation("ProviderSourceNavigation");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Sample", b =>
                {
                    b.HasOne("Cadorinator.Infrastructure.Entity.ProjectionsSchedule", "ProjectionsSchedule")
                        .WithMany("Samples")
                        .HasForeignKey("ProjectionsScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectionsSchedule");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Theater", b =>
                {
                    b.HasOne("Cadorinator.Infrastructure.Entity.Provider", "ProviderNavigation")
                        .WithMany("Theaters")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProviderNavigation");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.City", b =>
                {
                    b.Navigation("Providers");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Film", b =>
                {
                    b.Navigation("ProjectionsSchedules");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.ProjectionsSchedule", b =>
                {
                    b.Navigation("Samples");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.Provider", b =>
                {
                    b.Navigation("ProjectionsSchedules");

                    b.Navigation("Theaters");
                });

            modelBuilder.Entity("Cadorinator.Infrastructure.Entity.ProviderSource", b =>
                {
                    b.Navigation("Providers");
                });
#pragma warning restore 612, 618
        }
    }
}
