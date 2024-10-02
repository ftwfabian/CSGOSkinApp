﻿// <auto-generated />
using System;
using CSGOSkinApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CSGOSkinApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240929201352_UpdateSkinNameToString")]
    partial class UpdateSkinNameToString
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("CSGOSkinApp.Entities.Skin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Float")
                        .HasColumnType("REAL");

                    b.Property<decimal>("MarketPrice")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<string>("Weapon")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("dateListed")
                        .HasColumnType("TEXT");

                    b.Property<string>("sticker1")
                        .HasColumnType("TEXT");

                    b.Property<string>("sticker2")
                        .HasColumnType("TEXT");

                    b.Property<string>("sticker3")
                        .HasColumnType("TEXT");

                    b.Property<string>("sticker4")
                        .HasColumnType("TEXT");

                    b.Property<string>("sticker5")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Skins");
                });
#pragma warning restore 612, 618
        }
    }
}
