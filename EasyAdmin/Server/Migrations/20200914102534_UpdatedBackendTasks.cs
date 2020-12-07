using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyAdmin.Server.Migrations
{
    public partial class UpdatedBackendTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "BackendTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "BackendTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "BackendTasks");

            migrationBuilder.DropColumn(
                name: "State",
                table: "BackendTasks");
        }
    }
}
