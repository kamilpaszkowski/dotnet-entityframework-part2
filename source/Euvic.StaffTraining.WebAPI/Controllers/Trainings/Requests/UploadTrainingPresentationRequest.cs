using Microsoft.AspNetCore.Http;

namespace Euvic.StaffTraining.WebAPI.Controllers.Requests
{
    public class UploadTrainingPresentationRequest
    {
        public IFormFile Presentation { get; set; }
    }
}
