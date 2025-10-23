using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyTalk.Backend.Data;
using EasyTalk.Shared.Models;


namespace EasyTalk.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CourseController(AppDbContext context) => _context = context;

        [HttpGet]

        //get all
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var query = _context.Courses
       .Include(c => c.Trainer)
       .AsQueryable();

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
                    TrainerName = c.Trainer != null ? c.Trainer.Name : null
                })
                .ToListAsync();

            return Ok(data);
        }
        // get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _context.Courses.Include(c => c.Trainer)
                .FirstOrDefaultAsync(c => c.Id == id);
            return course == null ? NotFound() : Ok(course);
        }

        //Post/Insert
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // setelah simpan, muat kembali Course lengkap dengan Trainer
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
                    TrainerName = c.Trainer != null ? c.Trainer.Name : null
                })
                .FirstOrDefaultAsync();
            return Ok(result);
        }

        //update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Course course)
        {
            if (id != course.Id) return BadRequest();
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(course);
        }
        

        //delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
