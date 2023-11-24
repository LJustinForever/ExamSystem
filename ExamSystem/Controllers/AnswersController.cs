using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ExamSystem.Controllers
{
    [Route("api/Exams/{examId}/Questions/{questionId}/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class AnswersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswersController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.ADMIN_AND_USER)]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswer(Guid examId, Guid questionId)
        {
            if(!ExamExists(examId))
                return NotFound();
            if (!QuestionExists(questionId))
                return NotFound();
            if (_context.Answer == null)
          {
              return NotFound();
          }
            return await _context.Answer.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.ADMIN)]
        public async Task<ActionResult<Answer>> GetAnswer(Guid examId, Guid questionId, Guid id)
        {
            if (!ExamExists(examId))
                return NotFound();
            if (!QuestionExists(questionId))
                return NotFound();
         
            var answer = await _context.Answer.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.ADMIN)]
        public async Task<IActionResult> PutAnswer(Guid examId, Guid questionId, Guid id, Answer answerEdit)
        {
            if (!ExamExists(examId))
                return NotFound();
            if (!QuestionExists(questionId))
                return NotFound();
            if (id != answerEdit.Id)
            {
                return BadRequest();
            }

            Answer? answer = await _context.Answer.FindAsync(id);
            if(answer == null) 
                return Problem("Answer not found");

            answer.Title = answerEdit.Title;
            answer.Description = answerEdit.Description;
            answer.IsCorrect= answerEdit.IsCorrect;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("GetAnswer", new { id });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.ADMIN)]
        public async Task<ActionResult<Answer>> PostAnswer(Guid examId, Guid questionId, Answer answer)
        {
            ApplicationUser? user = _context.Users.FirstOrDefault(e => e.Id == Guid.Parse(_userManager.GetUserId(HttpContext.User)));
            if (user == null)
                return NotFound();

            if (!ExamExists(examId))
                return NotFound();
            if (!QuestionExists(questionId))
                return NotFound();

            answer.QuestionId = questionId;
            answer.UserId = user.Id;
            _context.Answer.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.ADMIN)]
        public async Task<IActionResult> DeleteAnswer(Guid examId, Guid questionId, Guid id)        

        {
            if (!ExamExists(examId))
                return NotFound();
            if (!QuestionExists(questionId))
                return NotFound();
            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnswerExists(Guid id)
        {
            return (_context.Answer?.Any(e => e.Id == id)).GetValueOrDefault();
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
