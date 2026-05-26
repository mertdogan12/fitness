using Fitness.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<KursTermin> KurseTermine { get; set; }
        public DbSet<Kurs> Kurse { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NimmtTeil> NimmtTeil { get; set; }
        public DbSet<Trainer> Trainer { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public async Task<KursTerminDto?> getSingleTermin(int id)
        {
            KursTermin? termin = await Set<KursTermin>()
                .Include(k => k.Kurs)
                .SingleOrDefaultAsync(k => k.Id == id);

            if (termin == null) 
                return null;

            Dictionary<int, int> counts = await NimmtTeil
                .Where(n => termin.Id == n.KursTerminId)
                .GroupBy(n => n.KursTerminId)
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Id, x => x.Count);

            return new KursTerminDto
            {
                Id = termin.Id,
                KursId = termin.KursId,
                Kurs = termin.Kurs,
                TrainerID = termin.TrainerID,
                Anfang = termin.Anfang,
                MaxTeilnehmer = termin.MaxTeilnehmer,
                TeilnehmerAnzahl = counts.TryGetValue(termin.Id, out var c) ? c : 0
            };
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KursTermin>()
                .HasOne(k => k.Kurs)
                .WithMany()
                .HasForeignKey(k => k.KursId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
