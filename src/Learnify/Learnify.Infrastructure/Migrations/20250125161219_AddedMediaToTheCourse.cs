using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMediaToTheCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Courses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "Courses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_VideoId",
                table: "Courses",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_FileDatas_PhotoId",
                table: "Courses",
                column: "PhotoId",
                principalTable: "FileDatas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_FileDatas_VideoId",
                table: "Courses",
                column: "VideoId",
                principalTable: "FileDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_FileDatas_PhotoId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_FileDatas_VideoId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_VideoId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Courses");
        }
    }
}
