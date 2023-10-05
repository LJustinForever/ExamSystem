using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExamSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExamLocationsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ExamLocationsController(AppDbContext context)
        {
            _dbContext = context;
        }

        // GET: api/ExamLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamLocation>>> GetExamLocation()
        {
          if (_dbContext.ExamLocation == null)
          {
              return NotFound();
          }
            return await _dbContext.ExamLocation.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamLocation>> GetExamLocation(Guid id)
        {
          if (_dbContext.ExamLocation == null)
          {
              return NotFound();
          }
            var examLocation = await _dbContext.ExamLocation.FindAsync(id);

            if (examLocation == null)
            {
                return NotFound();
            }

            return examLocation;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditExamLocation(Guid id, [FromForm] ExamLocation examLocationEdit)
        {
            if (id != examLocationEdit.Id)
            {
                return BadRequest();
            }

            ExamLocation? examLocation = await _dbContext.ExamLocation.FindAsync(id);
            if (examLocation == null)
                return Problem("Exam Location not found");

            examLocation.Name = examLocationEdit.Name;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamLocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("GetExamLocation", new { id });
        }

        [HttpPost]
        public async Task<ActionResult<ExamLocation>> PostExamLocation(ExamLocation examLocation)
        {
            _dbContext.ExamLocation.Add(examLocation);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetExamLocation", new { id = examLocation.Id }, examLocation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamLocation(Guid id)
        {
            var examLocation = await _dbContext.ExamLocation.FindAsync(id);
            if (examLocation == null)
            {
                return NotFound();
            }

            _dbContext.ExamLocation.Remove(examLocation);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamLocationExists(Guid id)
        {
            return (_dbContext.ExamLocation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
