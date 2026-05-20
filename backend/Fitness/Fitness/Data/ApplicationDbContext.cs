using Fitness.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<KursTermin> KurseTermine { get; set; }
        public DbSet<Kurs> Kurse { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
