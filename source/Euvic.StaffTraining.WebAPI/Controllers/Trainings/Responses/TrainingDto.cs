using System;

namespace Euvic.StaffTraining.WebAPI.Dto
{
    public class TrainingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime TrainingDate { get; set; }
        public long LecturerId { get; set; }
        public long TechnologyId { get; set; }
        public int ConfirmedAttendances { get; set; }
    }
}
