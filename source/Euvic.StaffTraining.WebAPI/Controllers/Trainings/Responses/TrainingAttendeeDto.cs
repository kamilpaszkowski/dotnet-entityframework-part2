namespace Euvic.StaffTraining.WebAPI.Controllers.Trainings.Responses
{
    public class TrainingAttendeeDto
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
    }
}
