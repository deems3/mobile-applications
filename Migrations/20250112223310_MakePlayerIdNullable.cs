using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruthOrDrinkDemiBruls.Migrations
{
    /// <inheritdoc />
    public partial class MakePlayerIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Players_PlayerId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Questions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Players_PlayerId",
                table: "Questions",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Players_PlayerId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Players_PlayerId",
                table: "Questions",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
