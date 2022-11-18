using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SupportAPI.Auth.Model;
using SupportAPI.Data.Entities;

namespace SupportAPI.Data
{
    public class DbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Ticket>()
                .HasOne(o => o.TicketStatus)
                .WithMany(o => o.Tickets)
                .HasForeignKey(o => o.TicketStatusId);
        }
    }
}
