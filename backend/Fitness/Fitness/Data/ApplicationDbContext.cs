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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public async Task<KursTermin?> getSingleTermin(int id)
        {
            return await Set<KursTermin>()
                .Include(k => k.Kurs)
                .SingleOrDefaultAsync(k => k.Id == id);
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
