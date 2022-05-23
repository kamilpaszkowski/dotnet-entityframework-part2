using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euvic.StaffTraining.WebAPI.Migrations
{
    public partial class AddUserProviderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserProviderId",
                table: "Lecturers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserProviderId",
                table: "Attendees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProviderId",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "UserProviderId",
                table: "Attendees");
        }
    }
}
