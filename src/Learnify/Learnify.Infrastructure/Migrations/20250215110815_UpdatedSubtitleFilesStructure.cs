using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSubtitleFilesStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_FileDatas_FileId",
                table: "Subtitles");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Subtitles",
                newName: "TranscriptionFileId");

            migrationBuilder.RenameIndex(
                name: "IX_Subtitles_FileId",
                table: "Subtitles",
                newName: "IX_Subtitles_TranscriptionFileId");

            migrationBuilder.AddColumn<int>(
                name: "SubtitleFileId",
                table: "Subtitles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subtitles_SubtitleFileId",
                table: "Subtitles",
                column: "SubtitleFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_FileDatas_SubtitleFileId",
                table: "Subtitles",
                column: "SubtitleFileId",
                principalTable: "FileDatas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_FileDatas_TranscriptionFileId",
                table: "Subtitles",
                column: "TranscriptionFileId",
                principalTable: "FileDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_FileDatas_SubtitleFileId",
                table: "Subtitles");

            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_FileDatas_TranscriptionFileId",
                table: "Subtitles");

            migrationBuilder.DropIndex(
                name: "IX_Subtitles_SubtitleFileId",
                table: "Subtitles");

            migrationBuilder.DropColumn(
                name: "SubtitleFileId",
                table: "Subtitles");

            migrationBuilder.RenameColumn(
                name: "TranscriptionFileId",
                table: "Subtitles",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Subtitles_TranscriptionFileId",
                table: "Subtitles",
                newName: "IX_Subtitles_FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_FileDatas_FileId",
                table: "Subtitles",
                column: "FileId",
                principalTable: "FileDatas",
                principalColumn: "Id");
        }
    }
}
