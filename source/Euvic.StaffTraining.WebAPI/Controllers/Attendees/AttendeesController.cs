using System.Linq;
using System.Threading.Tasks;
using Euvic.StaffTraining.Identity.Abstractions;
using Euvic.StaffTraining.WebAPI.Controllers.Attendees.Requests;
using Euvic.StaffTraining.WebAPI.Dto;
using Euvic.StaffTraining.WebAPI.Entities;
using Euvic.StaffTraining.WebAPI.Entities.Enums;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Euvic.StaffTraining.WebAPI.Controllers
{
    [Route("api/attendees")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly ILogger<AttendeesController> _logger;
        private readonly StaffTrainingContext _context;
        private readonly IIdentityProvider _identityProvider;

        public AttendeesController(ILogger<AttendeesController> logger, StaffTrainingContext context, IIdentityProvider identityProvider)
        {
            _logger = logger;
            _context = context;
            _identityProvider = identityProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendies()
        {
            var attendees = await _context.Attendees
                 .Include(x => x.Trainings)
                     .ThenInclude(x => x.Training)
                 .Select(x =>
                 new AttendeeDto()
                 {
                     Id = x.Id,
                     AllowedHours = x.AllowedHours,
                     Firstname = x.Firstname,
                     Lastname = x.Lastname,
                     TotalHours = (double)x.Trainings.Where(x => x.StatusId != (int)TrainingAttendeeStatuses.Declined).Sum(t => t.Training.Duration) / 60,
                     TotalConfirmedHours = (double)x.Trainings.Where(x => x.StatusId == (int)TrainingAttendeeStatuses.Confirmed).Sum(t => t.Training.Duration) / 60,
                 })
                 .OrderBy(x => x.Lastname)
                 .ThenBy(x => x.Firstname)
                 .ToListAsync();

            return Ok(attendees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendee([FromBody] CreateAttendeeRequest request)
        {
            var attendee = new Attendee()
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                AllowedHours = 8
            };

            _context.Attendees.Add(attendee);

            attendee.UserProviderId = await _identityProvider.CreateUserAsync(request.Email, request.Password);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendee([FromRoute] long id, [FromBody] UpdateAttendeeRequest request)
        {
            var attendee = await _context.Attendees.FirstOrDefaultAsync(x => x.Id == id);
            if (attendee == null)
                return NotFound();

            attendee.Firstname = request.Firstname;
            attendee.Lastname = request.Lastname;
            attendee.AllowedHours = request.AllowedHours;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Atendee with id {id} has been updated");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendee([FromRoute] long id)
        {
            var attendee = await _context.Attendees.FirstOrDefaultAsync(x => x.Id == id);

            if (attendee == null)
                return NotFound();

            attendee.IsDeleted = true;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Atendee with id {id} has been deleted");

            return NoContent();
        }
    }
}
