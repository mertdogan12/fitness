using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Controllers
{
    [Route("api/[controller]")]
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

            return Ok(buchung);
        }
    }
}
