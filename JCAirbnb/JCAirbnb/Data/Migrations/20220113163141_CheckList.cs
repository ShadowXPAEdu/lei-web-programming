using Microsoft.EntityFrameworkCore.Migrations;

namespace JCAirbnb.Data.Migrations
{
    public partial class CheckList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "CheckLists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_CompanyId",
                table: "CheckLists",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckLists_Companies_CompanyId",
                table: "CheckLists",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckLists_Companies_CompanyId",
                table: "CheckLists");

            migrationBuilder.DropIndex(
                name: "IX_CheckLists_CompanyId",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CheckLists");
        }
    }
}
