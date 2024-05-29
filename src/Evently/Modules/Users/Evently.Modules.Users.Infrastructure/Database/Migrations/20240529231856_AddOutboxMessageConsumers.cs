using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Modules.Users.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessageConsumers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "outbox_message_consumers",
                schema: "users",
                columns: table => new
                {
                    outbox_message_id = table.Column<Guid>(type: "uuid", maxLength: 500, nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_message_consumers", x => new { x.outbox_message_id, x.name });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_message_consumers",
                schema: "users");
        }
    }
}
