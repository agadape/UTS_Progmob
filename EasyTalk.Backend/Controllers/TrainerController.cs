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
        private readonly IWebHostEnvironment _env;

        public TrainerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var query = _context.Trainers.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Name.Contains(search));

            return Ok(await query.ToListAsync());
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            return trainer == null ? NotFound() : Ok(trainer);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return Ok(trainer);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Trainer trainer)
        {
            if (id != trainer.Id)
                return BadRequest();

            var existing = await _context.Trainers.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existing == null)
                return NotFound();

            // cek apakah foto diganti
            if (!string.IsNullOrEmpty(existing.ProfileImageUrl) &&
                existing.ProfileImageUrl != trainer.ProfileImageUrl)
            {
                DeleteImageFile(existing.ProfileImageUrl);
            }

            _context.Entry(trainer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(trainer);
        }

        // DELETE TRAINER
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            // hapus file foto jika ada
            if (!string.IsNullOrEmpty(trainer.ProfileImageUrl))
            {
                DeleteImageFile(trainer.ProfileImageUrl);
            }

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // --- HELPER UNTUK HAPUS FILE FOTO DARI WWWROOT ---
        private void DeleteImageFile(string imageUrl)
        {
            try
            {
                var relativePath = imageUrl.TrimStart('/');
                var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

                var fullPath = Path.Combine(webRoot, relativePath);

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeleteImageFile ERROR] {ex.Message}");
            }
        }
    }
}
