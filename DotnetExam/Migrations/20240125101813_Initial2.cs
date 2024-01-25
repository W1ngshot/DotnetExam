using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetExam.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_OpponentId",
                table: "Games");

            migrationBuilder.AlterColumn<Guid>(
                name: "OpponentId",
                table: "Games",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_OpponentId",
                table: "Games",
                column: "OpponentId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_OpponentId",
                table: "Games");

            migrationBuilder.AlterColumn<Guid>(
                name: "OpponentId",
                table: "Games",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_OpponentId",
                table: "Games",
                column: "OpponentId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
