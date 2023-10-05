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
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public QuestionsController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestion()
        {
          if (_dbContext.Question == null)
          {
              return NotFound();
          }
            return await _dbContext.Question.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(Guid id)
        {
          if (_dbContext.Question == null)
          {
              return NotFound();
          }
            var question = await _dbContext.Question.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditQuestion(Guid id,[FromForm] Question questionEdit)
        {
            if (id != questionEdit.Id)
            {
                return BadRequest();
            }

            var question = await _dbContext.Question.FirstAsync(x => x.Id == id);
            question.Number = questionEdit.Number;
            question.Description = questionEdit.Description;
            try
            {
                await _dbContext.SaveChangesAsync();
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

            return RedirectToAction("GetQuestion", new {id});
        }

        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion([FromForm]Question question)
        {
            _dbContext.Question.Add(question);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            if (_dbContext.Question == null)
            {
                return NotFound();
            }
            var question = await _dbContext.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _dbContext.Question.Remove(question);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(Guid id)
        {
            return (_dbContext.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
