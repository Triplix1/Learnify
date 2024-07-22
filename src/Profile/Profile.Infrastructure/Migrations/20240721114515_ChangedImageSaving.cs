using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedImageSaving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Users",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "PhotoPublicId",
                table: "Users",
                newName: "ImageContainerName");

            migrationBuilder.AddColumn<string>(
                name: "ImageBlobName",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBlobName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Users",
                newName: "PhotoUrl");

            migrationBuilder.RenameColumn(
                name: "ImageContainerName",
                table: "Users",
                newName: "PhotoPublicId");
        }
    }
}
