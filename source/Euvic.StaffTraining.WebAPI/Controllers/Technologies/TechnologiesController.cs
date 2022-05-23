using System.Linq;
using System.Threading.Tasks;
using Euvic.StaffTraining.WebAPI.Controllers.Technologies.Requests;
using Euvic.StaffTraining.WebAPI.Controllers.Technologies.Responses;
using Euvic.StaffTraining.WebAPI.Entities;
using Euvic.StaffTraining.WebAPI.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Euvic.StaffTraining.WebAPI.Controllers.Technologies
{
    [Route("api/technologies")]
    [ApiController]
    public class TechnologiesController : ControllerBase
    {
        private readonly ILogger<TechnologiesController> _logger;
        private readonly StaffTrainingContext _context;

        public TechnologiesController(ILogger<TechnologiesController> logger, StaffTrainingContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTechnologies()
        {
            var technologies = await _context.Technologies
                 .Select(x =>
                 new TechnologyDto()
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Scope = x.Scope
                 })
                 .ToListAsync();

            return Ok(technologies);
        }

        [HttpGet("{technologyId}/name")]
        public async Task<IActionResult> GetTechnologyName([FromRoute] int technologyId)
        {
            //var technology = await _context.Technologies.FirstOrDefaultAsync(x => x.Id == technologyId); // BAD
            //var technologyName = technology.Name;

            var technologyName = await _context.Technologies
                .Where(x => x.Id == technologyId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(); // GOOD 

            return Ok(technologyName);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTechnology([FromBody] CreateTechnologyRequest request)
        {
            var technology = new Technology()
            {
                Name = request.Name,
                Scope = request.Scope
            };

            _context.Technologies.Add(technology);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechnology([FromRoute] long id, [FromBody] UpdateTechnologyRequest request)
        {
            var technology = await _context.Technologies.FirstOrDefaultAsync(x => x.Id == id);
            if (technology == null)
                return NotFound();

            technology.Name = request.Name;
            technology.Scope = request.Scope;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTechnologyByAttach([FromRoute] long id, [FromBody] UpdateTechnologyRequest request)
        //{
        //    var technology = new Technology()
        //    {
        //        Id = id,
        //        Name = request.Name,
        //        Scope = request.Scope
        //    };

        //    _context.Technologies.Update(technology);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnology([FromRoute] long id)
        {
            var technology = await _context.Technologies.FirstOrDefaultAsync(x => x.Id == id);

            if (technology == null)
                return NotFound();

            _context.Technologies.Remove(technology);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Atendee with id {id} has been deleted");

            return NoContent();
        }
    }
}
