using Euvic.StaffTraining.WebAPI.Entities;

namespace Euvic.StaffTraining.WebAPI.Controllers.Technologies.Requests
{
    public class UpdateTechnologyRequest
    {
        public string Name { get; set; }
        public TechnologyScope Scope { get; set; }
    }
}
