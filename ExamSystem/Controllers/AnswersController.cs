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
    [Route("[controller]")]
    [ApiController]
    public class AnswersController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AnswersController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswer()
        {
          if (_dbContext.Answer == null)
          {
              return NotFound();
          }
            return await _dbContext.Answer.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(Guid id)
        {
          if (_dbContext.Answer == null)
          {
              return NotFound();
          }
            var answer = await _dbContext.Answer.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(Guid id, [FromForm] Answer answerEdit)
        {
            if (id != answerEdit.Id)
            {
                return BadRequest();
            }

            Answer? answer = await _dbContext.Answer.FindAsync(id);
            if(answer == null) 
                return Problem("Answer not found");

            answer.Title = answerEdit.Title;
            answer.Description = answerEdit.Description;
            answer.IsCorrect= answerEdit.IsCorrect;

            try
            {
                await _dbContext.SaveChangesAsync();
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
        public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
        {
            _dbContext.Answer.Add(answer);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(Guid id)
        {
            var answer = await _dbContext.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _dbContext.Answer.Remove(answer);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool AnswerExists(Guid id)
        {
            return (_dbContext.Answer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
