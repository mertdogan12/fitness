using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateKurs([FromBody]Kurs kurs)
        {
            _context.Kurse.Add(kurs);
            _context.SaveChanges();
            return CreatedAtAction(nameof(CreateKurs), new { id = kurs.Id }, kurs);
        }

        [HttpGet]
        public IActionResult GetKurse()
        {
            return Ok(_context.Kurse.ToList());
        }
    }
}
