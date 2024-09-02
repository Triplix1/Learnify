using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsPublishedParagraph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPublished",
                table: "Paragraphs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPublished",
                table: "Paragraphs");
        }
    }
}
