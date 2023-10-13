using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;

namespace ExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamTimesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExamTimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExamTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamTime>>> GetExamTime()
        {
          if (_context.ExamTime == null)
          {
              return NotFound();
          }
            return await _context.ExamTime.ToListAsync();
        }

        // GET: api/ExamTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamTime>> GetExamTime(Guid id)
        {
            if (_context.ExamTime == null)
            {
                return NotFound();
            }
            var examTime = _context.ExamTime.Include(e => e.Exam).Include(e => e.ExamLocation).FirstOrDefault(e => e.Id == id);

            if (examTime == null)
            {
                return NotFound();
            }

            return examTime;
        }

        // PUT: api/ExamTimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamTime(Guid id, ExamTime examTime)
        {
            var oldExamTime = await _context.ExamTime.FindAsync(id);
            if(oldExamTime == null)
                return NotFound();

            oldExamTime.Date = examTime.Date;
            oldExamTime.Time = examTime.Time;
            oldExamTime.Status = examTime.Status;
            oldExamTime.ExamLocationId = examTime.ExamLocationId;
            oldExamTime.ExamId = examTime.ExamId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamTimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExamTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExamTime>> PostExamTime(ExamTime examTime)
        {
          if (_context.ExamTime == null)
          {
              return Problem("Entity set 'AppDbContext.ExamTime'  is null.");
          }
            _context.ExamTime.Add(examTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamTime", new { id = examTime.Id }, examTime);
        }

        // DELETE: api/ExamTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamTime(Guid id)
        {
            if (_context.ExamTime == null)
            {
                return NotFound();
            }
            var examTime = await _context.ExamTime.FindAsync(id);
            if (examTime == null)
            {
                return NotFound();
            }

            _context.ExamTime.Remove(examTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamTimeExists(Guid id)
        {
            return (_context.ExamTime?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
