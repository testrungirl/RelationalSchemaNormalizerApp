﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RelationalSchemaNormalizerLibrary.Models;

#nullable disable

namespace RelationalSchemaNormalizerUI.Migrations
{
    [DbContext(typeof(RelationalSchemaNormalizerLibrary.Models.AppContext))]
    [Migration("20240917105208_Added erdImagePaths")]
    partial class AddederdImagePaths
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.AttributeDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AttributeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("KeyAttribute")
                        .HasColumnType("bit");

                    b.Property<string>("TableDetailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TableDetailId");

                    b.ToTable("AttributeDetails");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.DatabaseDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConnectionString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataBaseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DatabaseDetails");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.GenTableAttributeDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AttributeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GeneratedTableId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("KeyAttribute")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("GeneratedTableId");

                    b.ToTable("GenTableAttributeDetails");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.GeneratedTable", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LevelOfNF")
                        .HasColumnType("int");

                    b.Property<string>("TableDetailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TableDetailId");

                    b.ToTable("GeneratedTables");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.TableDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<string>("DatabaseDetailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImgPathFor2NF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgPathFor3NF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LevelOfNF")
                        .HasColumnType("int");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DatabaseDetailId");

                    b.ToTable("TableDetails");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.AttributeDetail", b =>
                {
                    b.HasOne("RelationalSchemaNormalizerLibrary.Models.TableDetail", "TableDetail")
                        .WithMany("AttributeDetails")
                        .HasForeignKey("TableDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TableDetail");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.GenTableAttributeDetail", b =>
                {
                    b.HasOne("RelationalSchemaNormalizerLibrary.Models.GeneratedTable", "GeneratedTable")
                        .WithMany("GenTableAttributeDetails")
                        .HasForeignKey("GeneratedTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GeneratedTable");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.GeneratedTable", b =>
                {
                    b.HasOne("RelationalSchemaNormalizerLibrary.Models.TableDetail", "TableDetail")
                        .WithMany("GeneratedTables")
                        .HasForeignKey("TableDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TableDetail");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.TableDetail", b =>
                {
                    b.HasOne("RelationalSchemaNormalizerLibrary.Models.DatabaseDetail", "DatabaseDetail")
                        .WithMany("TablesDetails")
                        .HasForeignKey("DatabaseDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DatabaseDetail");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.DatabaseDetail", b =>
                {
                    b.Navigation("TablesDetails");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.GeneratedTable", b =>
                {
                    b.Navigation("GenTableAttributeDetails");
                });

            modelBuilder.Entity("RelationalSchemaNormalizerLibrary.Models.TableDetail", b =>
                {
                    b.Navigation("AttributeDetails");

                    b.Navigation("GeneratedTables");
                });
#pragma warning restore 612, 618
        }
    }
}
