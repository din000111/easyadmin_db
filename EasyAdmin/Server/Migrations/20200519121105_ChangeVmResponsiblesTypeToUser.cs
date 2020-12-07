using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyAdmin.Server.Migrations
{
    public partial class ChangeVmResponsiblesTypeToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Vms");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Vms");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Vms");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Vms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Vms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Vms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Vms");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Vms");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Vms");

            migrationBuilder.AddColumn<string>(
                name: "Admin",
                table: "Vms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Vms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Vms",
                type: "text",
                nullable: true);
        }
    }
}
