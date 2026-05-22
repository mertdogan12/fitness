using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Controllers
{
    [Route("api/buchungen")]
    [ApiController]
    public class BuchungsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuchungsController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost("buchen")]
        public async Task<IActionResult> CreateBuchung([FromBody] NimmtTeil buchung)
        {
            _context.NimmtTeil.Add(buchung);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return BadRequest($"Fehler beim Erstellen der Buchung. {ex.Message}");
            }

            return Ok();
        }

        [HttpPost("stonieren")]
        public async Task<IActionResult> StroniereBuchung([FromBody] StonierungsDto buchung)
        {
            List<NimmtTeil> termin = await _context.NimmtTeil
                .Where(u => u != null && u.User.Name == buchung.Name && u.User.Vorname == buchung.Vorname && u.KursTerminId == buchung.terminID)
                .ToListAsync();

            if (termin.Count() == 0)
            {
                return NotFound();
            }

            foreach (var item in termin)
            {
                Console.WriteLine(item.KursTerminId);
                _context.NimmtTeil.Remove(item);

                try
                {
                    await _context.SaveChangesAsync();
                } catch(DbUpdateException ex)
                {
                    return BadRequest($"Error beim stonieren: {ex.Message}");
                }
            }

            return NoContent();
        }
    }
}
