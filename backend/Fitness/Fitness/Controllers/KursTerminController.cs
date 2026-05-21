using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fitness.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KursTerminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KursTerminController(ApplicationDbContext context) 
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTermin([FromBody] KursTermin termin)
        {
            _context.KurseTermine.Add(termin);
            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return BadRequest($"Fehler beim Erstellen des Termins. {ex.Message}");
            }

            return CreatedAtAction(nameof(CreateTermin), new { id = termin.Id }, await _context.getSingleTermin(termin.Id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKursTermin(int id)
        {
            KursTermin? termin = await _context.getSingleTermin(id);

            if (termin == null)
            {
                return NotFound($"Termin mit der ID {id} existiert nicht");
            }

            return Ok(termin);
        }

        [HttpGet]
        public async Task<IActionResult> GetKurseAb([FromQuery] DateTime? von, [FromQuery] DateTime? bis = null)
        {
            if (von == null)
                return BadRequest("Query-Parameter 'ab' ist erforderlich (z. B. 2026-05-21 oder 2026-05-21T14:00:00).");

            var termine = await _context.KurseTermine
                .Include(t => t.Kurs)
                .Where(t => t.Anfang.HasValue
                            && t.Anfang.Value >= von.Value
                            && (bis == null || t.Anfang.Value <= bis.Value))
                .ToListAsync();

            return Ok(termine);
        }
    }
}
