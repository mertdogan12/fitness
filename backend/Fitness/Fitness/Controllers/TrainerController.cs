using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainers()
        {
            return Ok(await _context.Trainer.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainers(int id)
        {
            Trainer? trainer = await _context.Trainer.FindAsync(id);

            if (trainer == null)
            {
                return NotFound($"Trainer mit der ID {id} konnte nicht gefunden werden");
            }

            return Ok(trainer);
        }
    }
}
