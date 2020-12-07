using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyAdmin.Server.Migrations
{
    public partial class UpdatedBackendTasks2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelatedEntityName",
                table: "BackendTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedEntityType",
                table: "BackendTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedEntityName",
                table: "BackendTasks");

            migrationBuilder.DropColumn(
                name: "RelatedEntityType",
                table: "BackendTasks");
        }
    }
}
