﻿// <auto-generated />
using System;
using ClinicaMedicaWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClinicaMedicaWeb.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211004003534_migracaoInicial")]
    partial class migracaoInicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClinicaMedicaWeb.Models.Administrador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TBAdministrador");
                });

            modelBuilder.Entity("ClinicaMedicaWeb.Models.Login", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdministradorID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MedicoID")
                        .HasColumnType("int");

                    b.Property<int?>("SecretariaID")
                        .HasColumnType("int");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("ID");

                    b.HasIndex("AdministradorID");

                    b.HasIndex("MedicoID");

                    b.HasIndex("SecretariaID");

                    b.ToTable("TBLogin");
                });

            modelBuilder.Entity("ClinicaMedicaWeb.Models.Medico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CRM")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CPF")
                        .IsUnique();

                    b.HasIndex("CRM")
                        .IsUnique();

                    b.ToTable("TBMedico");
                });

            modelBuilder.Entity("ClinicaMedicaWeb.Models.Secretaria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CPF")
                        .IsUnique();

                    b.ToTable("TBSecretaria");
                });

            modelBuilder.Entity("ClinicaMedicaWeb.Models.Login", b =>
                {
                    b.HasOne("ClinicaMedicaWeb.Models.Administrador", "Administrador")
                        .WithMany()
                        .HasForeignKey("AdministradorID");

                    b.HasOne("ClinicaMedicaWeb.Models.Medico", "Medico")
                        .WithMany()
                        .HasForeignKey("MedicoID");

                    b.HasOne("ClinicaMedicaWeb.Models.Secretaria", "Secretaria")
                        .WithMany()
                        .HasForeignKey("SecretariaID");

                    b.Navigation("Administrador");

                    b.Navigation("Medico");

                    b.Navigation("Secretaria");
                });
#pragma warning restore 612, 618
        }
    }
}