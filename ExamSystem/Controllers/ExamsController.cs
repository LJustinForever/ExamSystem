using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;
using Microsoft.AspNetCore.Authorization;

namespace ExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExamsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Exams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exam>>> GetExam(int pageNumber)
        {
          if (_context.Exam == null)
          {
              return NotFound();
          }
            return await _context.Exam.Include(e => e.Questions).Take(10 * pageNumber).ToListAsync();
        }

        // GET: api/Exams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exam>> GetExam(Guid id)
        {
            var exam = await _context.Exam.Include(e => e.Questions).ThenInclude(e => e.Answers).FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }

        // PUT: api/Exams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExam(Guid id, Exam exam)
        {
            var oldExam = await _context.Exam.FindAsync(id);
            if (oldExam == null)
                return NotFound();
            using (var context = _context.Database.BeginTransaction()) 
            {
                oldExam.Name = exam.Name;
                oldExam.Number = exam.Number;
                oldExam.Status = exam.Status;
                _context.SaveChanges();
                context.Commit();
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamExists(id))
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

        // POST: api/Exams
        [HttpPost]
        public async Task<ActionResult<Exam>> PostExam(Exam exam)
        {
          if (_context.Exam == null)
          {
              return Problem("Entity set 'AppDbContext.Exam'  is null.");
          }
            _context.Exam.Add(exam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExam", new { id = exam.Id }, exam);
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            if (_context.Exam == null)
            {
                return NotFound();
            }
            var exam = await _context.Exam.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            _context.Exam.Remove(exam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamExists(Guid id)
        {
            return (_context.Exam?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
