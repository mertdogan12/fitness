using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KursController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateKurs([FromBody]Kurs kurs)
        {
            _context.Kurse.Add(kurs);
            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return BadRequest($"Fehler beim Erstellen des Kurses. {ex.Message}");
            }

            return CreatedAtAction(nameof(CreateKurs), new { id = kurs.Id }, kurs);
        }

        [HttpGet]
        public async Task<IActionResult> GetKurse()
        {
            return Ok(await _context.Kurse.ToListAsync());
        }
    }
}
