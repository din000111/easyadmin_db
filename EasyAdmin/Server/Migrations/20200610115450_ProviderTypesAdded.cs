using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyAdmin.Server.Migrations
{
    public partial class ProviderTypesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProviderType",
                table: "Providers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProviderType",
                value: 1);

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "Id", "Name", "ProviderType", "Version" },
                values: new object[] { 2, "VMware", 2, "6.7" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ProviderType",
                table: "Providers");
        }
    }
}
