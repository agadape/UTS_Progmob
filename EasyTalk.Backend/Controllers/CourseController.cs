using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyTalk.Backend.Data;
using EasyTalk.Shared.Models;

namespace EasyTalk.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var query = _context.Courses.Include(c => c.Trainer).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.Title.Contains(search));

            var data = await query
                .Select(c => new Course
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    DurationHours = c.DurationHours,
                    TrainerId = c.TrainerId,
                    TrainerName = c.Trainer != null ? c.Trainer.Name : null,
                    CoverImageUrl = c.CoverImageUrl
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Trainer)
                .FirstOrDefaultAsync(c => c.Id == id);

            return course == null ? NotFound() : Ok(course);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // Return course with trainer name
            var result = await _context.Courses
                .Include(c => c.Trainer)
                .Where(c => c.Id == course.Id)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    DurationHours = c.DurationHours,
                    TrainerId = c.TrainerId,
                    TrainerName = c.Trainer != null ? c.Trainer.Name : null,
                    CoverImageUrl = c.CoverImageUrl
                })
                .FirstOrDefaultAsync();

            return Ok(result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Course updated)
        {
            if (id != updated.Id)
                return BadRequest();

            var existing = await _context.Courses.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existing == null)
                return NotFound();

            // Jika cover diganti → hapus file lama
            if (!string.IsNullOrEmpty(existing.CoverImageUrl) &&
                existing.CoverImageUrl != updated.CoverImageUrl)
            {
                DeleteImageFile(existing.CoverImageUrl);
            }

            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(updated);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            if (!string.IsNullOrEmpty(course.CoverImageUrl))
            {
                DeleteImageFile(course.CoverImageUrl);
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // 🔥 HELPER: Hapus file cover image dari wwwroot
        private void DeleteImageFile(string url)
        {
            try
            {
                var relative = url.TrimStart('/');
                var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                var full = Path.Combine(root, relative);

                if (System.IO.File.Exists(full))
                    System.IO.File.Delete(full);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeleteImageFile] ERROR: {ex.Message}");
            }
        }
    }
}
