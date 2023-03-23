using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxExample.Contracts.Persistence.Models;

namespace OutboxExample.Contracts.Persistence.Mappings
{
    public class ReservationMapping : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable(nameof(Reservation));
            builder.Property(p => p.PeopleCount).IsRequired();
            builder.Property(p => p.CustomerId).IsRequired();
            builder.Property(p => p.StartFrom).IsRequired();
            builder.Property(p => p.EndIn).IsRequired();
            builder.Property(p => p.TableNo).IsRequired();
        }
    }
}
