﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MQTTCloud.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MQTTCloud.Migrations
{
    [DbContext(typeof(MessageContext))]
    [Migration("20191121213227_second")]
    partial class second
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MQTTCloud.Models.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Airtime")
                        .HasColumnType("integer");

                    b.Property<string>("DevId")
                        .HasColumnType("text");

                    b.Property<string>("PayloadRaw")
                        .HasColumnType("text");

                    b.Property<int>("Rssi")
                        .HasColumnType("integer");

                    b.Property<float>("Snr")
                        .HasColumnType("real");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
