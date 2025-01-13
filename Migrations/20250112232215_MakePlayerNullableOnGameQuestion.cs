using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruthOrDrinkDemiBruls.Migrations
{
    /// <inheritdoc />
    public partial class MakePlayerNullableOnGameQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameQuestions_Players_PlayerId",
                table: "GameQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "GameQuestions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_GameQuestions_Players_PlayerId",
                table: "GameQuestions",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameQuestions_Players_PlayerId",
                table: "GameQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "GameQuestions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameQuestions_Players_PlayerId",
                table: "GameQuestions",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
