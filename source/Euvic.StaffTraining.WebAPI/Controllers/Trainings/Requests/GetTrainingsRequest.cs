using System;

namespace Euvic.StaffTraining.WebAPI.Controllers.Requests
{
    public class GetTrainingsRequest
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public long? LecturerId { get; set; }
    }
}
