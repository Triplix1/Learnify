using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserQuizAnswerPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingStreams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuizAnswers",
                table: "UserQuizAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserQuizAnswers_UserId_LessonId",
                table: "UserQuizAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "LessonId",
                table: "UserQuizAnswers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuizAnswers",
                table: "UserQuizAnswers",
                columns: new[] { "UserId", "QuizId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_LessonId_UserId",
                table: "UserQuizAnswers",
                columns: new[] { "LessonId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBoughts_CourseId",
                table: "UserBoughts",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AuthorId",
                table: "Courses",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_AuthorId",
                table: "Courses",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBoughts_Courses_CourseId",
                table: "UserBoughts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBoughts_Users_UserId",
                table: "UserBoughts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_AuthorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBoughts_Courses_CourseId",
                table: "UserBoughts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBoughts_Users_UserId",
                table: "UserBoughts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuizAnswers",
                table: "UserQuizAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserQuizAnswers_LessonId_UserId",
                table: "UserQuizAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserBoughts_CourseId",
                table: "UserBoughts");

            migrationBuilder.DropIndex(
                name: "IX_Courses_AuthorId",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "LessonId",
                table: "UserQuizAnswers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuizAnswers",
                table: "UserQuizAnswers",
                columns: new[] { "UserId", "LessonId", "QuizId" });

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
                name: "IX_UserQuizAnswers_UserId_LessonId",
                table: "UserQuizAnswers",
                columns: new[] { "UserId", "LessonId" });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingStreams_ConnectionId",
                table: "MeetingStreams",
                column: "ConnectionId");
        }
    }
}
