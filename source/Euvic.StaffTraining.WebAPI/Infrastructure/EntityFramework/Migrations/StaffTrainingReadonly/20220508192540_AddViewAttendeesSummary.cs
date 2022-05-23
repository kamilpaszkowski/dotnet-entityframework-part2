using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euvic.StaffTraining.WebAPI.Migrations.StaffTrainingReadonly
{
    public partial class AddViewAttendeesSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
                CREATE VIEW [dbo].[AttendeeSummary]
                AS
                SELECT   dbo.Attendees.Id AS AttendeeId, SUM(dbo.Trainings.Duration) AS TotalHours
                FROM         dbo.Attendees
                INNER JOIN  dbo.TrainingAttendee ON dbo.Attendees.Id = dbo.TrainingAttendee.AttendeeId
                INNER JOIN dbo.Trainings ON dbo.TrainingAttendee.TrainingId = dbo.Trainings.Id
                GROUP BY dbo.Attendees.Id
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[AttendeeSummary]");
        }
    }
}
