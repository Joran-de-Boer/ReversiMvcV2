﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReversiMvcV2.DAL;

#nullable disable

namespace ReversiMvcV2.Migrations
{
    [DbContext(typeof(SpelerContext))]
    partial class SpelerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ReversiMvcV2.Models.Speler", b =>
                {
                    b.Property<string>("Guid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AantalGelijk")
                        .HasColumnType("int");

                    b.Property<int>("AantalGewonnen")
                        .HasColumnType("int");

                    b.Property<int>("AantalVerloren")
                        .HasColumnType("int");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("Spelers");
                });
#pragma warning restore 612, 618
        }
    }
}
