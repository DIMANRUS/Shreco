﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shreco.API.Data;

#nullable disable

namespace Shreco.API.Migrations.Lite
{
    [DbContext(typeof(LiteContext))]
    [Migration("20220208171233_InitDb")]
    partial class InitDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Shreco.API.Data.Session", b =>
                {
                    b.Property<string>("SessionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SessionId");

                    b.ToTable("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}