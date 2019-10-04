﻿// <auto-generated />
using System;
using IPSAS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IPSAS.Persistence.Migrations
{
    [DbContext(typeof(IPSASDbContext))]
    [Migration("20191004172526_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("IPSAS.Domain.Entities.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("GradeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HomeInstitution")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Matricule")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Phone")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SpecialityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.HasIndex("SpecialityId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("IPSAS.Domain.Entities.TeacherGrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TeacherGrade");
                });

            modelBuilder.Entity("IPSAS.Domain.Entities.TeacherSpeciality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TeacherSpeciality");
                });

            modelBuilder.Entity("IPSAS.Domain.Entities.Teacher", b =>
                {
                    b.HasOne("IPSAS.Domain.Entities.TeacherGrade", "Grade")
                        .WithMany()
                        .HasForeignKey("GradeId");

                    b.HasOne("IPSAS.Domain.Entities.TeacherSpeciality", "Speciality")
                        .WithMany()
                        .HasForeignKey("SpecialityId");
                });
#pragma warning restore 612, 618
        }
    }
}
