﻿// <auto-generated />
using FileUploadApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileUploadApplication.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240208063211_addingdriveid")]
    partial class addingdriveid
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FileUploadApplication.Entities.ImgData", b =>
                {
                    b.Property<int>("ImgDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImgDataId"));

                    b.Property<string>("Caption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DriveFileId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("VideoData")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImgDataId");

                    b.HasIndex("UserId");

                    b.ToTable("ImgDatas");
                });

            modelBuilder.Entity("FileUploadApplication.Entities.UserData", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserDatas");
                });

            modelBuilder.Entity("FileUploadApplication.Entities.ImgData", b =>
                {
                    b.HasOne("FileUploadApplication.Entities.UserData", "UserData")
                        .WithMany("ImgDataList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserData");
                });

            modelBuilder.Entity("FileUploadApplication.Entities.UserData", b =>
                {
                    b.Navigation("ImgDataList");
                });
#pragma warning restore 612, 618
        }
    }
}