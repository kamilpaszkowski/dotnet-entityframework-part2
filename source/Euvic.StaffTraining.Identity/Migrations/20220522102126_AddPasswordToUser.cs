using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euvic.StaffTraining.Identity.Migrations
{
    public partial class AddPasswordToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                schema: "identity",
                table: "Users");
        }
    }
}
