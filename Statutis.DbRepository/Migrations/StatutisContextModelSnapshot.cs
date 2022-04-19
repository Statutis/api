﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Statutis.DbRepository;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    [DbContext(typeof(StatutisContext))]
    partial class StatutisContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GroupTeam", b =>
                {
                    b.Property<Guid>("GroupsGroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeamsTeamId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupsGroupId", "TeamsTeamId");

                    b.HasIndex("TeamsTeamId");

                    b.ToTable("GroupTeam");
                });

            modelBuilder.Entity("Statutis.Entity.History.HistoryEntry", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string>("message")
                        .HasColumnType("text");

                    b.HasKey("ServiceId", "DateTime");

                    b.ToTable("History");
                });

            modelBuilder.Entity("Statutis.Entity.Service.Group", b =>
                {
                    b.Property<Guid>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("MainGroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("GroupId");

                    b.HasIndex("MainGroupId");

                    b.HasIndex("Name", "MainGroupId")
                        .IsUnique();

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Statutis.Entity.Service.Service", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("ServiceTypeName")
                        .IsRequired()
                        .HasColumnType("character varying(30)");

                    b.HasKey("ServiceId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ServiceTypeName");

                    b.HasIndex("Name", "GroupId")
                        .IsUnique();

                    b.ToTable("Service");
                });

            modelBuilder.Entity("Statutis.Entity.Service.ServiceType", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Color")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<byte[]>("Icon")
                        .HasColumnType("bytea");

                    b.HasKey("Name");

                    b.ToTable("ServiceType");

                    b.HasData(
                        new
                        {
                            Name = "Site Web"
                        },
                        new
                        {
                            Name = "SMTP"
                        },
                        new
                        {
                            Name = "DNS"
                        },
                        new
                        {
                            Name = "SSH"
                        },
                        new
                        {
                            Name = "Serveur"
                        });
                });

            modelBuilder.Entity("Statutis.Entity.Team", b =>
                {
                    b.Property<Guid>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<Guid?>("MainTeamId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("TeamId");

                    b.HasIndex("MainTeamId");

                    b.HasIndex("Name", "MainTeamId")
                        .IsUnique();

                    b.ToTable("Team");
                });

            modelBuilder.Entity("Statutis.Entity.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("bytea");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Email");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.Property<Guid>("TeamsTeamId")
                        .HasColumnType("uuid");

                    b.Property<string>("UsersEmail")
                        .HasColumnType("character varying(50)");

                    b.HasKey("TeamsTeamId", "UsersEmail");

                    b.HasIndex("UsersEmail");

                    b.ToTable("TeamUser");
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.DnsService", b =>
                {
                    b.HasBaseType("Statutis.Entity.Service.Service");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("DnsService", (string)null);
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.HttpService", b =>
                {
                    b.HasBaseType("Statutis.Entity.Service.Service");

                    b.Property<int?>("Code")
                        .HasColumnType("integer");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<string>("RedirectUrl")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.ToTable("HttpService", (string)null);
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.PingService", b =>
                {
                    b.HasBaseType("Statutis.Entity.Service.Service");

                    b.ToTable("PingService", (string)null);
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.SshService", b =>
                {
                    b.HasBaseType("Statutis.Entity.Service.Service");

                    b.Property<string>("Bash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsSshKey")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.ToTable("SshService", (string)null);
                });

            modelBuilder.Entity("GroupTeam", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Statutis.Entity.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Statutis.Entity.History.HistoryEntry", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Service", "Service")
                        .WithMany("HistoryEntries")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Statutis.Entity.Service.Group", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Group", "MainGroup")
                        .WithMany("Children")
                        .HasForeignKey("MainGroupId");

                    b.Navigation("MainGroup");
                });

            modelBuilder.Entity("Statutis.Entity.Service.Service", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Group", "Group")
                        .WithMany("Services")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Statutis.Entity.Service.ServiceType", "ServiceType")
                        .WithMany()
                        .HasForeignKey("ServiceTypeName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("ServiceType");
                });

            modelBuilder.Entity("Statutis.Entity.Team", b =>
                {
                    b.HasOne("Statutis.Entity.Team", "MainTeam")
                        .WithMany("Children")
                        .HasForeignKey("MainTeamId");

                    b.Navigation("MainTeam");
                });

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.HasOne("Statutis.Entity.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Statutis.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UsersEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.DnsService", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Service", null)
                        .WithOne()
                        .HasForeignKey("Statutis.Entity.Service.Check.DnsService", "ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.HttpService", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Service", null)
                        .WithOne()
                        .HasForeignKey("Statutis.Entity.Service.Check.HttpService", "ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.PingService", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Service", null)
                        .WithOne()
                        .HasForeignKey("Statutis.Entity.Service.Check.PingService", "ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.SshService", b =>
                {
                    b.HasOne("Statutis.Entity.Service.Service", null)
                        .WithOne()
                        .HasForeignKey("Statutis.Entity.Service.Check.SshService", "ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Statutis.Entity.Service.Group", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("Statutis.Entity.Service.Service", b =>
                {
                    b.Navigation("HistoryEntries");
                });

            modelBuilder.Entity("Statutis.Entity.Team", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
