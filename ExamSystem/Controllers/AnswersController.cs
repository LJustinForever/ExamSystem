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
    [Route("api/Exams/{examId}/Questions/{questionId}/[controller]")]
    [ApiController]
    public class AnswersController : Controller
    {
        private readonly AppDbContext _context;

        public AnswersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
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
        public async Task<ActionResult<Answer>> PostAnswer(Guid examId, Guid questionId, Answer answer)
        {
            if (!ExamExists(examId))
                return NotFound();
            if (!QuestionExists(questionId))
                return NotFound();

            answer.QuestionId = questionId;
            _context.Answer.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        }

        [HttpDelete("{id}")]
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
