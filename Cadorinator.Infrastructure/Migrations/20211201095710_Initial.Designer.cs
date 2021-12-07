﻿// <auto-generated />
using System;
using Cadorinator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cadorinator.Infrastructure.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20211201095710_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("Cadorinator.Models.DataModel.Film", b =>
                {
                    b.Property<int>("FilmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FilmName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FirstProjectionDate")
                        .HasColumnType("TEXT");

                    b.HasKey("FilmId");

                    b.HasIndex("FilmId")
                        .IsUnique();

                    b.ToTable("Film");
                });
#pragma warning restore 612, 618
        }
    }
}