using Euvic.StaffTraining.WebAPI.Entities;

namespace Euvic.StaffTraining.WebAPI.Controllers.Technologies.Requests
{
    public class CreateTechnologyRequest
    {
        public string Name { get; set; }
        public TechnologyScope Scope { get; set; }
    }
}
