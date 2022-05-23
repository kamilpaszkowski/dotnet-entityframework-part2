using System;

namespace Euvic.StaffTraining.WebAPI.Controllers.Requests
{
    public class UpdateTrainingRequest
    {
        public DateTime StartTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public long LecturerId { get; set; }
        public int TechnologyId { get; set; }
    }
}
