using Microsoft.EntityFrameworkCore.Migrations;

namespace JCAirbnb.Data.Migrations
{
    public partial class PropertyCommodity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyCommodity_Commodities_CommodityId",
                table: "PropertyCommodity");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyCommodity_Properties_PropertyId",
                table: "PropertyCommodity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyCommodity",
                table: "PropertyCommodity");

            migrationBuilder.RenameTable(
                name: "PropertyCommodity",
                newName: "PropertyCommodities");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyCommodity_PropertyId",
                table: "PropertyCommodities",
                newName: "IX_PropertyCommodities_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyCommodity_CommodityId",
                table: "PropertyCommodities",
                newName: "IX_PropertyCommodities_CommodityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyCommodities",
                table: "PropertyCommodities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyCommodities_Commodities_CommodityId",
                table: "PropertyCommodities",
                column: "CommodityId",
                principalTable: "Commodities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyCommodities_Properties_PropertyId",
                table: "PropertyCommodities",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyCommodities_Commodities_CommodityId",
                table: "PropertyCommodities");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyCommodities_Properties_PropertyId",
                table: "PropertyCommodities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyCommodities",
                table: "PropertyCommodities");

            migrationBuilder.RenameTable(
                name: "PropertyCommodities",
                newName: "PropertyCommodity");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyCommodities_PropertyId",
                table: "PropertyCommodity",
                newName: "IX_PropertyCommodity_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyCommodities_CommodityId",
                table: "PropertyCommodity",
                newName: "IX_PropertyCommodity_CommodityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyCommodity",
                table: "PropertyCommodity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyCommodity_Commodities_CommodityId",
                table: "PropertyCommodity",
                column: "CommodityId",
                principalTable: "Commodities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyCommodity_Properties_PropertyId",
                table: "PropertyCommodity",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
