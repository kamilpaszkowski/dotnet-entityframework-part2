using Euvic.StaffTraining.WebAPI.Entities;

namespace Euvic.StaffTraining.WebAPI.Controllers.Lecturers.Requests
{
    public class GetLecturersRequest
    {
        public string SearchPhase { get; set; }
        public TechnologyScope? Scope { get; set; }
    }
}
