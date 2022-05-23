namespace Euvic.StaffTraining.WebAPI.Controllers.Attendees.Requests
{
    public class CreateAttendeeRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
