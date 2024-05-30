using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Modules.Attendance.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEventStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "event_statistics",
                schema: "attendance",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    starts_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ends_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tickets_sold = table.Column<int>(type: "integer", nullable: false),
                    attendees_checked_in = table.Column<int>(type: "integer", nullable: false),
                    duplicate_check_in_tickets = table.Column<List<string>>(type: "text[]", nullable: false),
                    invalid_check_in_tickets = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_statistics", x => x.event_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_statistics",
                schema: "attendance");
        }
    }
}
