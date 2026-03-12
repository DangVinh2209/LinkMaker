using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkMaker.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeUrlIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Urls_NewLinkId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "NewLinkId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Urls_NewLinkId",
                table: "Users",
                column: "NewLinkId",
                principalTable: "Urls",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Urls_NewLinkId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "NewLinkId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Urls_NewLinkId",
                table: "Users",
                column: "NewLinkId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
