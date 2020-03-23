﻿// <auto-generated />
using System;
using BuildingRegistry.Projections.Syndication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingRegistry.Projections.Syndication.Migrations
{
    [DbContext(typeof(SyndicationContext))]
    [Migration("20200323085337_ChangeVersieType")]
    partial class ChangeVersieType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DesiredState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.HasKey("Name")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ProjectionStates","BuildingRegistrySyndication");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Syndication.Address.AddressPersistentLocalIdItem", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<string>("PersistentLocalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("IsComplete", "IsRemoved");

                    b.ToTable("AddressPersistentLocalIdSyndication","BuildingRegistrySyndication");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Syndication.Parcel.BuildingParcelLatestItem", b =>
                {
                    b.Property<Guid>("ParcelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CaPaKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParcelId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("CaPaKey");

                    b.ToTable("BuildingParcelLatestItems","BuildingRegistrySyndication");
                });
#pragma warning restore 612, 618
        }
    }
}
