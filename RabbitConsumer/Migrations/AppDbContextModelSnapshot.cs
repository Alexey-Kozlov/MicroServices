﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RabbitConsumer.Persistance;

#nullable disable

namespace RabbitConsumer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RabbitConsumer.Domain.LogMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ActionName");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Date");

                    b.Property<string>("TypeData")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("TypeData");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("TypeName");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("UserName");

                    b.HasKey("Id")
                        .HasName("PK_LogMessage_id");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_LogMessage_Id");

                    b.ToTable("LogMessage", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
