﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OcrWebService.Data;

#nullable disable

namespace OcrWebService.Migrations
{
    [DbContext(typeof(DbFileContext))]
    [Migration("20240125094747_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OcrWebService.Data.Entity.OcrResultEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BlobId")
                        .HasColumnType("uuid");

                    b.Property<float?>("Confidence")
                        .HasColumnType("real");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("InputExtension")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("OutputExtension")
                        .HasColumnType("text");

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("OcrResults");
                });
#pragma warning restore 612, 618
        }
    }
}