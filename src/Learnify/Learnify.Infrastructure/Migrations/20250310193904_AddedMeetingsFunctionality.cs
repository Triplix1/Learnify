using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMeetingsFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_FileDatas_TranscriptionFileId",
                table: "Subtitles");

            migrationBuilder.DropIndex(
                name: "IX_Subtitles_TranscriptionFileId",
                table: "Subtitles");

            migrationBuilder.DropColumn(
                name: "TranscriptionFileId",
                table: "Subtitles");

            migrationBuilder.CreateTable(
                name: "MeetingSessions",
                columns: table => new
                {
                    SessionId = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_MeetingSessions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeetingConnections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IsAuthor = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingConnections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_MeetingConnections_MeetingSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "MeetingSessions",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK_MeetingConnections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeetingStreams",
                columns: table => new
                {
                    StreamId = table.Column<string>(type: "text", nullable: false),
                    ConnectionId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingStreams", x => x.StreamId);
                    table.ForeignKey(
                        name: "FK_MeetingStreams_MeetingConnections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "MeetingConnections",
                        principalColumn: "ConnectionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingConnections_SessionId",
                table: "MeetingConnections",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingConnections_UserId",
                table: "MeetingConnections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingSessions_CourseId",
                table: "MeetingSessions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingStreams_ConnectionId",
                table: "MeetingStreams",
                column: "ConnectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingStreams");

            migrationBuilder.DropTable(
                name: "MeetingConnections");

            migrationBuilder.DropTable(
                name: "MeetingSessions");

            migrationBuilder.AddColumn<int>(
                name: "TranscriptionFileId",
                table: "Subtitles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subtitles_TranscriptionFileId",
                table: "Subtitles",
                column: "TranscriptionFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_FileDatas_TranscriptionFileId",
                table: "Subtitles",
                column: "TranscriptionFileId",
                principalTable: "FileDatas",
                principalColumn: "Id");
        }
    }
}
