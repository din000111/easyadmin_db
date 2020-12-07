using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EasyAdmin.Server.Migrations
{
    public partial class AddedBackendTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackendTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BackendTaskId = table.Column<string>(nullable: true),
                    AdapterId = table.Column<int>(nullable: false),
                    IsEnded = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsVisible = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackendTasks_Adapters_AdapterId",
                        column: x => x.AdapterId,
                        principalTable: "Adapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackendTasks_AdapterId",
                table: "BackendTasks",
                column: "AdapterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackendTasks");
        }
    }
}
