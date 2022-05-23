namespace Euvic.StaffTraining.WebAPI.Dto
{
    public class AttendeeDto
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int AllowedHours { get; set; }
        public double TotalHours { get; set; }
        public double TotalConfirmedHours { get; set; }
    }
}
