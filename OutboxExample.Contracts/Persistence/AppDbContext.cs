using MassTransit;
using Microsoft.EntityFrameworkCore;
using OutboxExample.Contracts.Persistence.Mappings;
using OutboxExample.Contracts.Persistence.Models;

namespace OutboxExample.Contracts.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ReservationMapping());
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
            modelBuilder.AddInboxStateEntity();
        }
    }
}
