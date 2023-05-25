using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArshaWebApp.Migrations
{
    public partial class UpdateJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Jobs_JobId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Jobs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobId",
                table: "Jobs",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Jobs_JobId",
                table: "Jobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
