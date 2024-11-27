﻿// <auto-generated />
using System;
using EmployeeLoginDetails.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmployeeLoginDetails.Migrations
{
    [DbContext(typeof(UserLoginDbContext))]
    partial class UserLoginDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EmployeeLoginDetails.Models.LoginDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("Break")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("CheckIn")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("CheckOut")
                        .HasColumnType("interval");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("WorkingHours")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("EmployeeLoginDetails");
                });

            modelBuilder.Entity("EmployeeLoginDetails.Models.UserRegistrationRequest", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserID");

                    b.ToTable("UserLogin");
                });
#pragma warning restore 612, 618
        }
    }
}