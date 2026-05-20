using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateTermin([FromBody] KursTermin termin)
        {
            _context.KurseTermine.Add(termin);
            _context.SaveChanges();
            return CreatedAtAction(nameof(CreateTermin), new { id = termin.Id }, termin);
        }

        [HttpGet]
        public IActionResult GetKursTermine()
        {
            return Ok(_context.KurseTermine.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetKursTermin(int id)
        {
            var kurs = _context.KurseTermine.Find(id);

            if (kurs == null)
            {
                return NotFound();
            }

            return Ok(kurs);
        }
    }
}
