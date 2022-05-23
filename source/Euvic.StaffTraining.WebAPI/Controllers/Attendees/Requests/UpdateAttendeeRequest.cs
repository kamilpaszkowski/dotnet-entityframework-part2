namespace Euvic.StaffTraining.WebAPI.Controllers.Attendees.Requests
{
    public class UpdateAttendeeRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int AllowedHours { get; set; }
    }
}
