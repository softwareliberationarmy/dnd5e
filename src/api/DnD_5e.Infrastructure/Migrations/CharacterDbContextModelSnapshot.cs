﻿// <auto-generated />
using System;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DnD_5e.Infrastructure.Migrations
{
    [DbContext(typeof(CharacterDbContext))]
    partial class CharacterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DnD_5e.Infrastructure.DataAccess.Entities.CharacterEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Charisma")
                        .HasColumnType("int");

                    b.Property<bool>("CharismaSaveProficiency")
                        .HasColumnType("bit");

                    b.Property<int>("Class")
                        .HasColumnType("int");

                    b.Property<int>("Constitution")
                        .HasColumnType("int");

                    b.Property<bool>("ConstitutionSaveProficiency")
                        .HasColumnType("bit");

                    b.Property<int>("Dexterity")
                        .HasColumnType("int");

                    b.Property<bool>("DexteritySaveProficiency")
                        .HasColumnType("bit");

                    b.Property<int>("ExperiencePoints")
                        .HasColumnType("int");

                    b.Property<int>("Intelligence")
                        .HasColumnType("int");

                    b.Property<bool>("IntelligenceSaveProficiency")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<int>("Race")
                        .HasColumnType("int");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<bool>("StrengthSaveProficiency")
                        .HasColumnType("bit");

                    b.Property<int>("Wisdom")
                        .HasColumnType("int");

                    b.Property<bool>("WisdomSaveProficiency")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("DnD_5e.Infrastructure.DataAccess.Entities.SkillProficiencyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CharacterId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("SkillProficiency");
                });

            modelBuilder.Entity("DnD_5e.Infrastructure.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DnD_5e.Infrastructure.DataAccess.Entities.CharacterEntity", b =>
                {
                    b.HasOne("DnD_5e.Infrastructure.DataAccess.Entities.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DnD_5e.Infrastructure.DataAccess.Entities.SkillProficiencyEntity", b =>
                {
                    b.HasOne("DnD_5e.Infrastructure.DataAccess.Entities.CharacterEntity", null)
                        .WithMany("SkillProficiencies")
                        .HasForeignKey("CharacterId");
                });
#pragma warning restore 612, 618
        }
    }
}
