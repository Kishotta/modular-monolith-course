﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Modules.Attendance.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "attendance");

            migrationBuilder.CreateTable(
                name: "attendees",
                schema: "attendance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attendees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                schema: "attendance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    table_name = table.Column<string>(type: "text", nullable: false),
                    occured_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    primary_key = table.Column<string>(type: "text", nullable: false),
                    old_values = table.Column<string>(type: "text", nullable: true),
                    new_values = table.Column<string>(type: "text", nullable: true),
                    affected_columns = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                schema: "attendance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    start_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "attendance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "jsonb", maxLength: 3000, nullable: false),
                    occured_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                schema: "attendance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    attendee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    used_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                    table.ForeignKey(
                        name: "fk_tickets_attendees_attendee_id",
                        column: x => x.attendee_id,
                        principalSchema: "attendance",
                        principalTable: "attendees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tickets_events_event_id",
                        column: x => x.event_id,
                        principalSchema: "attendance",
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tickets_attendee_id",
                schema: "attendance",
                table: "tickets",
                column: "attendee_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_code",
                schema: "attendance",
                table: "tickets",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tickets_event_id",
                schema: "attendance",
                table: "tickets",
                column: "event_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "tickets",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "attendees",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "events",
                schema: "attendance");
        }
    }
}