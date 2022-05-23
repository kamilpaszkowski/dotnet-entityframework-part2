using Euvic.StaffTraining.WebAPI.Entities;

namespace Euvic.StaffTraining.WebAPI.Controllers.Technologies.Responses
{
    public class TechnologyDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TechnologyScope Scope { get; set; }
    }
}
