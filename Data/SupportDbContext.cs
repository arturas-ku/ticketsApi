using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Entities;

namespace SupportAPI.Data
{
    public class SupportDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }

        public SupportDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(o => o.Project)
                .WithMany(o => o.Tickets)
                .HasForeignKey(o => o.ProjectId);

            modelBuilder.Entity<Ticket>()
                .HasOne(o => o.TicketType)
                .WithMany(o => o.Tickets)
                .HasForeignKey(o => o.TicketTypeId);

            modelBuilder.Entity<TicketComment>()
                .HasOne(o => o.Ticket)
                .WithMany(o => o.TicketComments)
                .HasForeignKey(o => o.TicketId);
        }
    }
}
