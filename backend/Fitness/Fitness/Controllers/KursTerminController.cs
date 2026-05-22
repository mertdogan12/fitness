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

        [HttpDelete]
        public async Task<IActionResult> DeteleteTermin(int id)
        {
            KursTermin? termin = await _context.KurseTermine.FindAsync(id);

            if (termin == null)
            {
                return NotFound($"Kurstermin mit der ID {id} konnte nicht gefunden werden");
            }

            _context.KurseTermine.Remove(termin);

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return BadRequest($"Error beim löschen des Kurstermines. {ex.Message}");
            }

            return NotFound(termin);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKursTermin(int id)
        {
            KursTerminDto? termin = await _context.getSingleTermin(id);

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
                return BadRequest("Query-Parameter 'von' ist erforderlich (z. B. 2026-05-21 oder 2026-05-21T14:00:00).");


            List<KursTermin> termine = await _context.KurseTermine
                .Include(t => t.Kurs)
                .Where(t => t.Anfang.HasValue
                            && t.Anfang.Value >= von.Value
                            && (bis == null || t.Anfang.Value <= bis.Value))
                .ToListAsync();

            Dictionary<int, int> counts = await _context.NimmtTeil
                .Where(n => termine.Select(ft => ft.Id).Contains(n.KursTerminId))
                .GroupBy(n => n.KursTerminId)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Id, x => x.Count);

            List<KursTerminDto> dtos = termine.Select(t => new KursTerminDto
            {
                Id = t.Id,
                KursId = t.KursId,
                Kurs = t.Kurs,
                TrainerID = t.TrainerID,
                Anfang = t.Anfang,
                MaxTeilnehmer = t.MaxTeilnehmer,
                TeilnehmerAnzahl = counts.TryGetValue(t.Id, out var c) ? c : 0
            }).ToList();

            return Ok(dtos);
        }
    }
}
