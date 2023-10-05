using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Model;

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
        public IActionResult Index()
        {
            var data = _dbContext.Exam.ToList();

            return Json(data);
        }

        [HttpGet("ExamGet")]
        public async Task<IActionResult> ExamGet(Guid? id)
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

        // GET: Exams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Exam exam)
        {
            if (ModelState.IsValid)
            {
                exam.Id = Guid.NewGuid();
                _dbContext.Add(exam);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exams/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _dbContext.Exam == null)
            {
                return NotFound();
            }

            var exam = await _dbContext.Exam.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Number,Name,Status,Id")] Exam exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(exam);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
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

        private bool ExamExists(Guid id)
        {
          return (_dbContext.Exam?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
