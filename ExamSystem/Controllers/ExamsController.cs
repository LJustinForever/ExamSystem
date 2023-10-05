using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExamSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ExamsController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_dbContext.Exam == null)
            {
                return NotFound();
            }
            return  Json(await _dbContext.Exam.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExam(Guid? id)
        {
            if (id == null || _dbContext.Exam == null)
            {
                return NotFound();
            }

            var exam = await _dbContext.Exam
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }

            return Json(exam);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Exam exam)
        {
            if (ModelState.IsValid)
            {
                exam.Id = Guid.NewGuid();
                exam.Number = _dbContext.Exam.Count() + 1;
                exam.Status = Enums.ExamStatusEnum.INACTIVE;
                _dbContext.Add(exam);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (_dbContext.Exam == null)
            {
                return Problem("Entity set 'AppDbContext.Exam'  is null.");
            }
            var exam = await _dbContext.Exam.FindAsync(id);
            if (exam != null)
            {
                _dbContext.Exam.Remove(exam);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(Guid? id, [FromForm] Exam examEdit)
        {
            var exam = await _dbContext.Exam.FindAsync(id);

            if (exam == null)
            {
                return Problem("Exam not found");
            }

            exam.Name = examEdit.Name;
            exam.Questions= examEdit.Questions;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPut("Publish/{id}")]
        public async Task<IActionResult> Publish(Guid? id)
        {
            var exam = await _dbContext.Exam.FirstAsync(e => e.Id == id);

            if (exam == null)
            {
                return Problem("Exam not found");
            }
            if(exam.Questions.Count == 0) 
            {
                return Problem("No Questions");
            }
            foreach (Question question in exam.Questions)
            {
                bool containsCorrect = false;
                foreach (Answer answer in question.Answers)
                {
                    if(answer.IsCorrect)
                        containsCorrect = true;
                }
                if (!containsCorrect)
                    return Problem("Question does not contain a single correct answer");
            }

            exam.Status = Enums.ExamStatusEnum.ACTIVE;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
