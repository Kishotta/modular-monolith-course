﻿// <auto-generated />
using System;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Evently.Modules.Users.Infrastructure.Database.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    partial class UsersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("users")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Evently.Common.Infrastructure.Auditing.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AffectedColumns")
                        .HasColumnType("text")
                        .HasColumnName("affected_columns");

                    b.Property<string>("NewValues")
                        .HasColumnType("text")
                        .HasColumnName("new_values");

                    b.Property<DateTime>("OccuredAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occured_at_utc");

                    b.Property<string>("OldValues")
                        .HasColumnType("text")
                        .HasColumnName("old_values");

                    b.Property<string>("PrimaryKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("primary_key");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("table_name");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_audit_logs");

                    b.ToTable("audit_logs", "users");
                });

            modelBuilder.Entity("Evently.Modules.Users.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", "users");
                });
#pragma warning restore 612, 618
        }
    }
}
