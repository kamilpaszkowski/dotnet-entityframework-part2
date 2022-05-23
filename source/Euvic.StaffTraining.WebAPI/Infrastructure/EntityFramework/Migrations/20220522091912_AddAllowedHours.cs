using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euvic.StaffTraining.WebAPI.Migrations
{
    public partial class AddAllowedHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllowedHours",
                table: "Lecturers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllowedHours",
                table: "Attendees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedHours",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "AllowedHours",
                table: "Attendees");
        }
    }
}
