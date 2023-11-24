using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

namespace ExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamLocationsController : Controller
    {
        private readonly AppDbContext _context;

        public ExamLocationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExamLocations
        [HttpGet]
        [Authorize(Roles = UserRoles.ADMIN_AND_USER)]
        public async Task<ActionResult<IEnumerable<ExamLocation>>> GetExamLocation()
        {
          if (_context.ExamLocation == null)
          {
              return NotFound();
          }
            return await _context.ExamLocation.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.ADMIN_AND_USER)]
        public async Task<ActionResult<ExamLocation>> GetExamLocation(Guid id)
        {
          if (_context.ExamLocation == null)
          {
              return NotFound();
          }
            var examLocation = await _context.ExamLocation.FindAsync(id);

            if (examLocation == null)
            {
                return NotFound();
            }

            return examLocation;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.ADMIN)]
        public async Task<IActionResult> EditExamLocation(Guid id, [FromForm] ExamLocation examLocationEdit)
        {
            ExamLocation? examLocation = await _context.ExamLocation.FindAsync(id);
            if (examLocation == null)
                return Problem("Exam Location not found");

            examLocation.Name = examLocationEdit.Name;
            examLocation.Status = examLocationEdit.Status;
            try
            {
                await _context.SaveChangesAsync();
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
        [Authorize(Roles = UserRoles.ADMIN)]
        public async Task<ActionResult<ExamLocation>> PostExamLocation(ExamLocation examLocation)
        {
            _context.ExamLocation.Add(examLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamLocation", new { id = examLocation.Id }, examLocation);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.ADMIN)]
        public async Task<IActionResult> DeleteExamLocation(Guid id)
        {
            var examLocation = await _context.ExamLocation.FindAsync(id);
            if (examLocation == null)
            {
                return NotFound();
            }

            _context.ExamLocation.Remove(examLocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamLocationExists(Guid id)
        {
            return (_context.ExamLocation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
