﻿// <auto-generated />
using System;
using ChristmasTreeManager.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChristmasTreeManager.Infrastructure.Sqlite.Migrations.Application
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240107220741_AddFormCount")]
    partial class AddFormCount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.CollectionTourEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Driver")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Staff")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TeamLeader")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Vehicle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CollectionTours", t =>
                        {
                            t.HasTrigger("CollectionTours_Trigger");
                        });
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.DistributionTourEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Staff")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DistributionTours", t =>
                        {
                            t.HasTrigger("DistributionTours_Trigger");
                        });
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.RegistrationEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Customer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Donation")
                        .HasColumnType("REAL");

                    b.Property<uint>("Housenumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HousenumberPostfix")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("RegistrationPointId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("TreeCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RegistrationPointId");

                    b.HasIndex("StreetId");

                    b.ToTable("Registrations", t =>
                        {
                            t.HasTrigger("Registrations_Trigger");
                        });
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.RegistrationPointEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RegistrationPoints", t =>
                        {
                            t.HasTrigger("RegistrationPoints_Trigger");
                        });
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.StreetEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CollectionTourId")
                        .HasColumnType("TEXT");

                    b.Property<uint>("CollectionTourOrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("DistributionTourFormCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DistributionTourId")
                        .HasColumnType("TEXT");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("HighestHouseNumber")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("LowestHouseNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CollectionTourId");

                    b.HasIndex("DistributionTourId");

                    b.ToTable("Streets", t =>
                        {
                            t.HasTrigger("Streets_Trigger");
                        });
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.RegistrationEntity", b =>
                {
                    b.HasOne("ChristmasTreeManager.Data.Application.RegistrationPointEntity", "RegistrationPoint")
                        .WithMany("Registrations")
                        .HasForeignKey("RegistrationPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChristmasTreeManager.Data.Application.StreetEntity", "Street")
                        .WithMany("Registrations")
                        .HasForeignKey("StreetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RegistrationPoint");

                    b.Navigation("Street");
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.StreetEntity", b =>
                {
                    b.HasOne("ChristmasTreeManager.Data.Application.CollectionTourEntity", "CollectionTour")
                        .WithMany("Streets")
                        .HasForeignKey("CollectionTourId");

                    b.HasOne("ChristmasTreeManager.Data.Application.DistributionTourEntity", "DistributionTour")
                        .WithMany("Streets")
                        .HasForeignKey("DistributionTourId");

                    b.Navigation("CollectionTour");

                    b.Navigation("DistributionTour");
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.CollectionTourEntity", b =>
                {
                    b.Navigation("Streets");
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.DistributionTourEntity", b =>
                {
                    b.Navigation("Streets");
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.RegistrationPointEntity", b =>
                {
                    b.Navigation("Registrations");
                });

            modelBuilder.Entity("ChristmasTreeManager.Data.Application.StreetEntity", b =>
                {
                    b.Navigation("Registrations");
                });
#pragma warning restore 612, 618
        }
    }
}
