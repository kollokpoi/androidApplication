using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomApi.Migrations
{
    /// <inheritdoc />
    public partial class addObjectImages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectImage_Objects_ObjectId",
                table: "ObjectImage");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectId",
                table: "ObjectImage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectImage_Objects_ObjectId",
                table: "ObjectImage",
                column: "ObjectId",
                principalTable: "Objects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectImage_Objects_ObjectId",
                table: "ObjectImage");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectId",
                table: "ObjectImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectImage_Objects_ObjectId",
                table: "ObjectImage",
                column: "ObjectId",
                principalTable: "Objects",
                principalColumn: "Id");
        }
    }
}
