using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euvic.StaffTraining.WebAPI.Migrations
{
    public partial class AddIsDeletedToAttendeesAndLecturers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lecturers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Attendees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attendees");
        }
    }
}
