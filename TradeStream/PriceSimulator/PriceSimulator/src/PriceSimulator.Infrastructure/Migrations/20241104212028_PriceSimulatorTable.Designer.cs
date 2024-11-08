﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PriceSimulator.Infrastructure.Adapters.Persistence.MySQL;

#nullable disable

namespace PriceSimulator.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241104212028_PriceSimulatorTable")]
    partial class PriceSimulatorTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.35")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.Asks", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid?>("OrderBookDataId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("OrderBookDataId");

                    b.ToTable("Asks");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.Bids", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid?>("OrderBookDataId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("OrderBookDataId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.OrderBook", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("DataId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DataId");

                    b.ToTable("OrderBooks");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.OrderBookData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("Microtimestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("OrderBookDatas");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.TradeStream.Price", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid?>("PriceSimulationId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("PriceSimulationId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.TradeStream.PriceSimulation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Cryptocurrency")
                        .HasColumnType("int");

                    b.Property<int>("OperationType")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("PriceSimulations");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.Asks", b =>
                {
                    b.HasOne("PriceSimulator.Domain.Entities.Bitstamp.OrderBookData", null)
                        .WithMany("Asks")
                        .HasForeignKey("OrderBookDataId");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.Bids", b =>
                {
                    b.HasOne("PriceSimulator.Domain.Entities.Bitstamp.OrderBookData", null)
                        .WithMany("Bids")
                        .HasForeignKey("OrderBookDataId");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.OrderBook", b =>
                {
                    b.HasOne("PriceSimulator.Domain.Entities.Bitstamp.OrderBookData", "Data")
                        .WithMany()
                        .HasForeignKey("DataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Data");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.TradeStream.Price", b =>
                {
                    b.HasOne("PriceSimulator.Domain.Entities.TradeStream.PriceSimulation", null)
                        .WithMany("PricesUsed")
                        .HasForeignKey("PriceSimulationId");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.Bitstamp.OrderBookData", b =>
                {
                    b.Navigation("Asks");

                    b.Navigation("Bids");
                });

            modelBuilder.Entity("PriceSimulator.Domain.Entities.TradeStream.PriceSimulation", b =>
                {
                    b.Navigation("PricesUsed");
                });
#pragma warning restore 612, 618
        }
    }
}
