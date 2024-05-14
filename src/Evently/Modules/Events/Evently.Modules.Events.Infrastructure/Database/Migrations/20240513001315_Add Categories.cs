using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Modules.Events.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                schema: "events",
                table: "events",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_category_id",
                schema: "events",
                table: "events",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_events_categories_category_id",
                schema: "events",
                table: "events",
                column: "category_id",
                principalSchema: "events",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_events_categories_category_id",
                schema: "events",
                table: "events");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "events");

            migrationBuilder.DropIndex(
                name: "ix_events_category_id",
                schema: "events",
                table: "events");

            migrationBuilder.DropColumn(
                name: "category_id",
                schema: "events",
                table: "events");
        }
    }
}
