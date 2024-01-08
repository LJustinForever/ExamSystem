using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExamSystem.Controllers
{
    [Route("api/Exams/{examId}/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser>  _userManager;
        private readonly SignInManager<ApplicationUser>  _signInManager;

        public QuestionsController(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestion(Guid examId, int pageNumber)
        {
            if (!ExamExists(examId))
                return NotFound();
            if (_context.Question == null)
                return NotFound();

            return await _context.Question.Include(e => e.Answers).Where(e => e.ExamId == examId).Take(10 * pageNumber).ToListAsync();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(Guid examId, Guid id)
        {
            if (!ExamExists(examId))
                return NotFound();
            if (_context.Question == null)
              return NotFound();
            var question = _context.Question.Include(e => e.Answers).Where(e => e.ExamId == examId).FirstOrDefault(e => e.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(Guid examId, Guid id, Question question)
        {
            if (!ExamExists(examId))
                return NotFound();
            if (!QuestionExists(id))
                return NotFound();
            var oldQuestion = _context.Question.FirstOrDefault(e => e.Id == id);

            oldQuestion.Number = question.Number; 
            oldQuestion.Description = question.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Guid examId, Question question)
        {
            Guid id = Guid.Parse(User.FindFirst("sub")?.Value);
            var user = _context.Users.FirstOrDefault(e => e.Id == id);
            if (!ExamExists(examId))
                return NotFound();
            if (_context.Question == null)
            {
                return Problem("Entity set 'AppDbContext.Question'  is null.");
            }

            question.ExamId = examId;
            question.UserId = user.Id;
            _context.Question.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid examId, Guid id)
        {
            if (!ExamExists(examId))
                return NotFound();
            if (_context.Question == null)
            {
                return NotFound();
            }
            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Question.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(Guid id)
        {
            return (_context.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool ExamExists(Guid id) 
        {
            return (_context.Exam?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
