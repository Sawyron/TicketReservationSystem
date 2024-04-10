using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketReservation.Persistence.TicketTypes;

internal class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
    {
        builder.ToTable("ticket_types");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(50);
        builder.HasIndex(t => t.Name)
            .IsUnique();
    }

    private static IEnumerable<TicketType> GetTicketTypes()
    {
        yield return new TicketType
        {
            Id = Guid.NewGuid(),
            Name = "Standard"
        };
        yield return new TicketType
        {
            Id = Guid.NewGuid(),
            Name = "Luxiary"
        };
    }
}
