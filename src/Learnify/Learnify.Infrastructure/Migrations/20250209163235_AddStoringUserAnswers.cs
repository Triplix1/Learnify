using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoringUserAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserQuizAnswers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    QuizId = table.Column<string>(type: "text", nullable: false),
                    LessonId = table.Column<string>(type: "text", nullable: false),
                    AnswerIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizAnswers", x => new { x.UserId, x.LessonId, x.QuizId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_UserId_LessonId",
                table: "UserQuizAnswers",
                columns: new[] { "UserId", "LessonId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQuizAnswers");
        }
    }
}
