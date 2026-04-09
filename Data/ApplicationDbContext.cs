using Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events => Set<Event>();
        public DbSet<Attendee> Attendees => Set<Attendee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Attendees)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}