using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Euvic.StaffTraining.WebAPI.Controllers.Requests;
using Euvic.StaffTraining.WebAPI.Dto;
using Euvic.StaffTraining.WebAPI.Entities;
using Euvic.StaffTraining.WebAPI.Entities.Enums;
using Euvic.StaffTraining.WebAPI.Exceptions;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Euvic.StaffTraining.WebAPI.Controllers
{
    [Route("api/trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ILogger<TrainingsController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly StaffTrainingContext _context;

        public TrainingsController(
            ILogger<TrainingsController> logger,
            IWebHostEnvironment environment,
            StaffTrainingContext context)
        {
            _logger = logger;
            _environment = environment;
            _context = context;
        }

        #region Trainings

        [HttpGet]
        public async Task<IActionResult> GetTrainings([FromQuery] GetTrainingsRequest request)
        {
            var trainings = await _context.Trainings
                .Where(x => !request.From.HasValue || x.TrainingDate >= request.From)
                .Where(x => !request.To.HasValue || x.TrainingDate >= request.To)
                .Where(x => !request.LecturerId.HasValue || x.LecturerId == request.LecturerId)
                .Select(x => new TrainingDto()
                {
                    Id = x.Id,
                    LecturerId = x.LecturerId,
                    TrainingDate = x.TrainingDate,
                    CreateDate = x.CreateDate,
                    Duration = x.Duration,
                    Description = x.Description,
                    TechnologyId = x.TechnologyId,
                    Title = x.Title,
                    ConfirmedAttendances = x.Attendees.Where(x => x.StatusId == (int)TrainingAttendeeStatuses.Confirmed).Count()
                })
                .OrderBy(x => x.TrainingDate)
                .ToListAsync();

            return Ok(trainings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTraining([FromBody] CreateTrainingRequest request)
        {
            var training = new Training()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Duration = request.Duration,
                TrainingDate = request.StartTime,
                LecturerId = request.LecturerId,
                TechnologyId = request.TechnologyId,
            };

            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();

            return Ok(new TrainingDto()
            {
                Id = training.Id,
                TechnologyId = training.TechnologyId,
                Title = training.Title,
                LecturerId = training.LecturerId,
                TrainingDate = training.TrainingDate,
                CreateDate = training.CreateDate,
                Description = training.Description,
                Duration = training.Duration,
            });
        }

        [HttpPost("{id}/presentation")]
        public async Task<IActionResult> UploadTrainingPresentation([FromForm] UploadTrainingPresentationRequest request)
        {
            var rootPath = _environment.ContentRootPath;

            using (var stream = System.IO.File.Create(Path.Combine(rootPath, $"uploaded_{request.Presentation.FileName}")))
            {
                await request.Presentation.CopyToAsync(stream);
            }

            // Upload presentation code here

            _logger.LogInformation($"Training presentation for file {request.Presentation.FileName} was uploaded");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTraining([FromRoute] Guid id, [FromBody] UpdateTrainingRequest request)
        {
            var training = await _context.Trainings.FirstOrDefaultAsync(t => t.Id == id);

            if (training == null)
                return NotFound("Training not Found");

            training.LecturerId = request.LecturerId;
            training.Title = request.Title;
            training.Description = request.Description;
            training.Duration = request.Duration;
            training.TechnologyId = request.TechnologyId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/lecturer")]
        public async Task<IActionResult> ChangeTrainingLecturer([FromRoute] Guid id, [FromBody] long lecturerId)
        {
            var training = await _context.Trainings.FirstOrDefaultAsync(t => t.Id == id);
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == lecturerId);

            if (training == null)
                return NotFound("Training not Found");

            training.Lecturer = lecturer;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Training with id {id} changed lecturer to {lecturerId}");

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining([FromRoute] Guid id)
        {
            var training = _context.Trainings.FirstOrDefault(t => t.Id == id);

            if (training == null)
                return NotFound("Training not Found");

            _context.Trainings.Remove(training);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion Trainings

        #region Attendees

        [HttpGet("{id}/attendees")]
        public async Task<IActionResult> GetTrainingAttendees([FromRoute] Guid id, [FromQuery] int? statusId)
        {
            var attendees = await _context.Trainings
                .Include(x => x.Attendees)
                    .ThenInclude(x => x.Attendee)
                .Include(x => x.Attendees)
                    .ThenInclude(x => x.Status)
                .Where(x => x.Id == id)
                .SelectMany(x => x.Attendees)
                .Where(x => !statusId.HasValue || x.StatusId == statusId)
                .Select(x => new Trainings.Responses.TrainingAttendeeDto()
                {
                    Id = x.Attendee.Id,
                    Firstname = x.Attendee.Firstname,
                    Lastname = x.Attendee.Lastname,
                    Status = x.Status.Name,
                    StatusId = x.StatusId
                })
                .ToListAsync();

            return Ok(attendees);
        }

        [HttpGet("{id}/attendees/confirmed")]
        public async Task<IActionResult> GetConfirmedTrainingAttendees([FromRoute] Guid id)
        {
            var attendees = await _context.Trainings
                .Include(x => x.Attendees)
                      .ThenInclude(x => x.Attendee)
                .Include(x => x.Attendees)
                    .ThenInclude(x => x.Status)
                .Where(x => x.Id == id)
                .SelectMany(x => x.Attendees)
                .Where(x => x.IsConfirmed())
                .Select(x => new Trainings.Responses.TrainingAttendeeDto()
                {
                    Id = x.Attendee.Id,
                    Firstname = x.Attendee.Firstname,
                    Lastname = x.Attendee.Lastname,
                    Status = x.Status.Name,
                    StatusId = x.StatusId
                })
                .ToListAsync();

            return Ok(attendees);
        }

        [HttpPut("{id}/attendees")]
        public async Task<IActionResult> AddTrainingAttendee([FromRoute] Guid id, [FromBody] long attendeeId)
        {
            var training = await _context.Trainings.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == id);
            var attendee = await _context.Attendees.FirstOrDefaultAsync(x => x.Id == attendeeId);

            if (training == null)
                return NotFound("Training not found");

            if (attendee == null)
                return NotFound("Attendee not found");

            if (!training.Attendees.Any(x => x.AttendeeId == attendeeId))
            {
                training.Attendees.Add(new TrainingAttendee()
                {
                    AttendeeId = attendeeId,
                    StatusId = (int)TrainingAttendeeStatuses.Interested,
                });
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/attendees/{attendeeId}/confirm")]
        public async Task<IActionResult> ConfirmAttendee([FromRoute] Guid id, [FromRoute] long attendeeId)
        {
            var training = await _context.Trainings
                .Include(x => x.Attendees)
                .ThenInclude(x => x.Attendee)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (training == null)
                return NotFound("Training not Found");

            var trainingAttendee = training.Attendees.FirstOrDefault(x => x.AttendeeId == attendeeId);

            if (trainingAttendee == null)
                return NotFound("Atendee not Found");

            var totalConfirmedHours = _context.Attendees
                .Include(x => x.Trainings)
                    .ThenInclude(x => x.Training)
                .SelectMany(x => x.Trainings)
                .Where(x => x.StatusId == (int)TrainingAttendeeStatuses.Confirmed)
                .Sum(x => x.Training.Duration);

            if (totalConfirmedHours > trainingAttendee.Attendee.AllowedHours)
                throw new DomainException("You can't confirm anymore trainings because you have reached your allowed hours limit");

            trainingAttendee.StatusId = (int)TrainingAttendeeStatuses.Confirmed;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/attendees/{attendeeId}/unconfirm")]
        public async Task<IActionResult> UnconfirmAttendee([FromRoute] Guid id, [FromRoute] long attendeeId)
        {
            var training = await _context.Trainings.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == id);

            if (training == null)
                return NotFound("Training not Found");

            var trainingAttendee = training.Attendees.FirstOrDefault(x => x.AttendeeId == attendeeId);

            if (trainingAttendee == null)
                return NotFound("Atendee not Found");

            trainingAttendee.StatusId = (int)TrainingAttendeeStatuses.Interested;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/attendees/{attendeeId}/decline")]
        public async Task<IActionResult> DeclineAttendee([FromRoute] Guid id, [FromRoute] long attendeeId)
        {
            var training = await _context.Trainings.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == id);

            if (training == null)
                return NotFound("Training not Found");

            var trainingAttendee = training.Attendees.FirstOrDefault(x => x.AttendeeId == attendeeId);

            if (trainingAttendee == null)
                return NotFound("Atendee not Found");

            trainingAttendee.StatusId = (int)TrainingAttendeeStatuses.Declined;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/attendees/{attendeeId}")]
        public async Task<IActionResult> DeleteTrainingAttendees([FromRoute] Guid id, [FromRoute] long attendeeId)
        {
            var training = await _context.Trainings.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == id);

            var trainingAttendee = training.Attendees.FirstOrDefault(x => x.AttendeeId == attendeeId);
            training.Attendees.Remove(trainingAttendee);

            await _context.SaveChangesAsync();

            return Ok();
        }

        #endregion Attendees

    }
}
