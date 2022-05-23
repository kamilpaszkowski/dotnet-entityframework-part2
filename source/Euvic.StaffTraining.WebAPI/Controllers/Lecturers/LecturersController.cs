using System;
using System.Linq;
using System.Threading.Tasks;
using Euvic.StaffTraining.Identity.Abstractions;
using Euvic.StaffTraining.WebAPI.Controllers.Lecturers.Requests;
using Euvic.StaffTraining.WebAPI.Controllers.Lecturers.Responses;
using Euvic.StaffTraining.WebAPI.Entities;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Euvic.StaffTraining.WebAPI.Controllers.Lecturers
{
    [Route("api/lecturers")]
    [ApiController]
    public class LecturersController : ControllerBase
    {
        private readonly ILogger<LecturersController> _logger;
        private readonly StaffTrainingContext _context;
        private readonly IIdentityProvider _identityProvider;

        public LecturersController(ILogger<LecturersController> logger, StaffTrainingContext context, IIdentityProvider identityProvider)
        {
            _logger = logger;
            _context = context;
            _identityProvider = identityProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetLecturers([FromQuery] GetLecturersRequest request)
        {
            //var lecturers = await _context.Lecturers
            //     .Where(x => string.IsNullOrEmpty(request.SearchPhase) || (x.Firstname.Contains(request.SearchPhase) || x.Lastname.Contains(request.SearchPhase)))
            //     .Where(x => !request.Scope.HasValue || x.Trainings.Any(t => t.Technology.Scope == request.Scope))
            //     .Select(x =>
            //     new LecturerDto()
            //     {
            //         Id = x.Id,
            //         Firstname = x.Firstname,
            //         Lastname = x.Lastname
            //     })
            //     .ToListAsync(); // OPTION 1

            var lecturersQuery = _context.Lecturers.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchPhase))
                lecturersQuery = lecturersQuery.Where(x => x.Firstname.Contains(request.SearchPhase) || x.Lastname.Contains(request.SearchPhase));

            if (request.Scope.HasValue)
                lecturersQuery = lecturersQuery.Where(x => x.Trainings.Any(t => t.Technology.Scope == request.Scope));

            var lecturers = await lecturersQuery.Select(
                x => new LecturerDto()
                {
                    Id = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname
                })
            .ToListAsync(); // OPTION 2

            return Ok(lecturers);
        }

        [HttpGet("{lecturerId}/trainings/count")]
        public async Task<IActionResult> GetLecturerTrainingsCount([FromRoute] long lecturerId)
        {
            //var lecturer = await _context.Lecturers
            //    .Include(x => x.Trainings)
            //    .FirstOrDefaultAsync(x => x.Id == lecturerId);

            //var trainingsCount = lecturer.Trainings.Count(); // BAD

            var trainingsCount = await _context.Trainings
                .Where(x => x.LecturerId == lecturerId)
                .CountAsync();


            return Ok(trainingsCount);
        }

        [HttpGet("{lecturerId}/trainings/summary")]
        public async Task<IActionResult> GetLecturerTrainingsSummary([FromRoute] long lecturerId)
        {
            var lecturerSummary = await _context.Lecturers
                .Include(x => x.Trainings)
                .Where(x => x.Id == lecturerId)
                .SelectMany(x => x.Trainings)
                .GroupBy(x => x.Technology.Name)
                .Select(x => new { Technology = x.Key, Count = x.Count() })
                .ToDictionaryAsync(key => key.Technology, value => value.Count);

            return Ok(lecturerSummary);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLecturer([FromBody] CreateLecturerRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Add lecturer
                var lectuer = new Lecturer()
                {
                    Firstname = request.Firstname,
                    Lastname = request.Lastname
                };

                _context.Lecturers.Add(lectuer);

                await _context.SaveChangesAsync();

                // Each lectuer is also attendee
                var attendee = new Attendee()
                {
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    AllowedHours = 8
                };

                _context.Attendees.Add(attendee);

                await _context.SaveChangesAsync();

                var userProviderId = await _identityProvider.CreateUserAsync(request.Email, request.Password);
                attendee.UserProviderId = userProviderId;
                lectuer.UserProviderId = userProviderId;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "It was an exception during creating attendee");
                await transaction.RollbackAsync();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecturer([FromRoute] long id, [FromBody] UpdateLecturerRequest request)
        {
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == id);
            if (lecturer == null)
                return NotFound();

            lecturer.Firstname = request.Firstname;
            lecturer.Lastname = request.Lastname;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturer([FromRoute] long id)
        {
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == id);

            if (lecturer == null)
                return NotFound();

            lecturer.IsDeleted = true;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Lecturer with id {id} has been deleted");

            return NoContent();
        }
    }
}
