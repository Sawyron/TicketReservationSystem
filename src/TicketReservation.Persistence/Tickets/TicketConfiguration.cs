using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketReservation.Persistence.Tickets;

internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.Train)
            .WithMany(t => t.Tickets)
            .HasForeignKey(t => t.TrainId)
            .IsRequired();
        builder.HasIndex(t => new { t.TrainId, t.PlaceNumber })
            .IsUnique();
        builder.HasOne(t => t.TicketType)
            .WithMany()
            .HasForeignKey(t => t.TypeId)
            .IsRequired();
    }
}
