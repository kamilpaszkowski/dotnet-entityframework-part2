namespace Euvic.StaffTraining.WebAPI.Controllers.Lecturers.Requests
{
    public class CreateLecturerRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
