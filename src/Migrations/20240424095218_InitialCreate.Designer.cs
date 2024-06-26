﻿// <auto-generated />
using System;
using AIGallery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AIGallery.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240424095218_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("AIGallery.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("AIGallery.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("ImageProviderId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ThumbnailData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("ImageProviderId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("AIGallery.ImageProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApiKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ImageProviders");
                });

            modelBuilder.Entity("AlbumImage", b =>
                {
                    b.Property<int>("AlbumId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImagesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AlbumId", "ImagesId");

                    b.HasIndex("ImagesId");

                    b.ToTable("AlbumImage");
                });

            modelBuilder.Entity("AIGallery.Image", b =>
                {
                    b.HasOne("AIGallery.ImageProvider", "ImageProvider")
                        .WithMany("Images")
                        .HasForeignKey("ImageProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImageProvider");
                });

            modelBuilder.Entity("AlbumImage", b =>
                {
                    b.HasOne("AIGallery.Album", null)
                        .WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AIGallery.Image", null)
                        .WithMany()
                        .HasForeignKey("ImagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AIGallery.ImageProvider", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
