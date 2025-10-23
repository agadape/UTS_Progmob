using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyTalk.Backend.Data;
using EasyTalk.Shared.Models;



namespace EasyTalk.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class TrainerController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TrainerController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var query = _context.Trainers.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Name.Contains(search));

            return Ok(await query.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            return trainer == null ? NotFound() : Ok(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return Ok(trainer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Trainer trainer)
        {
            if (id != trainer.Id) return BadRequest();
            _context.Entry(trainer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(trainer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
