﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Statutis.DbRepository;

#nullable disable

namespace Statutis.DbRepository.Migrations
{
    [DbContext(typeof(StatutisContext))]
    [Migration("20220423105626_AddUserAvatarContentType")]
    partial class AddUserAvatarContentType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasData(
                        new
                        {
                            GroupsGroupId = new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"),
                            TeamsTeamId = new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42")
                        },
                        new
                        {
                            GroupsGroupId = new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"),
                            TeamsTeamId = new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa")
                        },
                        new
                        {
                            GroupsGroupId = new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"),
                            TeamsTeamId = new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42")
                        },
                        new
                        {
                            GroupsGroupId = new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"),
                            TeamsTeamId = new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa")
                        });
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

                    b.Property<bool>("IsPublic")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("GroupId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Group");

                    b.HasData(
                        new
                        {
                            GroupId = new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"),
                            Description = "Groupe par défaut",
                            IsPublic = true,
                            Name = "Défaut"
                        },
                        new
                        {
                            GroupId = new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"),
                            Description = "Groupe par défaut",
                            IsPublic = true,
                            Name = "Statutis"
                        });
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("TeamId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Team");

                    b.HasData(
                        new
                        {
                            TeamId = new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42"),
                            Color = "#e74c3c",
                            Name = "Administrateur"
                        },
                        new
                        {
                            TeamId = new Guid("3074c258-5eb5-4598-aafd-26ab51e2fcfa"),
                            Color = "#34495e",
                            Name = "Default"
                        },
                        new
                        {
                            TeamId = new Guid("9de07aec-ce5d-43bc-a909-c648a3b4956a"),
                            Color = "#95a5a6",
                            Name = "Sub Team"
                        });
                });

            modelBuilder.Entity("Statutis.Entity.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("bytea");

                    b.Property<string>("AvatarContentType")
                        .HasColumnType("text");

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

                    b.HasData(
                        new
                        {
                            Email = "contact@silvain.eu",
                            Password = "$argon2id$v=19$m=1024,t=1,p=1$c29tZXNhbHQ$Wg1s/1X6O1f1ZQCQtAvkGaCMwteH+F2h6p6AGDM8om4",
                            Roles = "ROLE_ADMIN",
                            Username = "admin"
                        });
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

                    b.HasData(
                        new
                        {
                            TeamsTeamId = new Guid("582745b3-1c8c-4c89-a772-19f6d9102f42"),
                            UsersEmail = "contact@silvain.eu"
                        });
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

                    b.HasData(
                        new
                        {
                            ServiceId = new Guid("e847af71-cec3-4c0e-9dc1-e67ac54a0cb4"),
                            Description = "DNS A pour silvain.eu",
                            GroupId = new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"),
                            Host = "silvain.eu",
                            Name = "DNS Silvain.eu",
                            ServiceTypeName = "DNS",
                            Result = "89.234.182.183",
                            Type = "A"
                        });
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

                    b.HasData(
                        new
                        {
                            ServiceId = new Guid("cbb3d7eb-0c88-46eb-9370-112e90271659"),
                            Description = "Serveur frontend de statutis",
                            GroupId = new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"),
                            Host = "https://statutis.silvain.eu",
                            Name = "Frontend",
                            ServiceTypeName = "Site Web",
                            Code = 200,
                            Port = 443
                        },
                        new
                        {
                            ServiceId = new Guid("0cac5b5a-fc9f-4894-849b-2b2b97538c2e"),
                            Description = "Serveur backend de statutis",
                            GroupId = new Guid("cad77a46-5c06-4741-b3ce-76d520d5b4ae"),
                            Host = "https://api.statutis.silvain.eu",
                            Name = "API",
                            ServiceTypeName = "Site Web",
                            Code = 404,
                            Port = 443
                        });
                });

            modelBuilder.Entity("Statutis.Entity.Service.Check.PingService", b =>
                {
                    b.HasBaseType("Statutis.Entity.Service.Service");

                    b.ToTable("PingService", (string)null);

                    b.HasData(
                        new
                        {
                            ServiceId = new Guid("3b0374ec-9a34-4e87-ade1-7fd3cc4e04f5"),
                            Description = "Serveur de google",
                            GroupId = new Guid("2395b8a3-1abb-4e2d-af1e-b3b830da10f9"),
                            Host = "8.8.8.8",
                            Name = "Serveur de Google",
                            ServiceTypeName = "Serveur"
                        });
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
                    b.Navigation("Services");
                });

            modelBuilder.Entity("Statutis.Entity.Service.Service", b =>
                {
                    b.Navigation("HistoryEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
