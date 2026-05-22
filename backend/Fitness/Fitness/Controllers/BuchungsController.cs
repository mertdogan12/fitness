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

        [HttpPost]
        public async Task<IActionResult> CreateBuchung([FromBody] NimmtTeil buchung)
        {
            KursTerminDto? termin = await _context.getSingleTermin(buchung.KursTerminId);

            if (termin == null)
                return NotFound($"Termin mit der ID {buchung.KursTerminId} existiert nicht");

            if (termin.MaxTeilnehmer == termin.TeilnehmerAnzahl)
                return BadRequest($"Kurs mit der ID {termin.KursId} ist schon voll");

            User user = buchung.User ?? await _context.Users.FindAsync(buchung.UserId);

            if (user == null)
                return NotFound($"User mit der ID {buchung.UserId} existiert nicht");

            if (termin.Kurs.Geschlecht != user.Geschlecht && termin.Kurs.Geschlecht != "alle")
                return BadRequest($"User muss das Geschlecht {termin.Kurs.Geschlecht} haben");

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

        [HttpDelete]
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
