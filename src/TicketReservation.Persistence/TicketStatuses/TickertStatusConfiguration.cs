using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketReservation.Persistence.TicketStatuses;

public class TicketStatusConfiguration : IEntityTypeConfiguration<TicketStatus>
{
    public void Configure(EntityTypeBuilder<TicketStatus> builder)
    {
        builder.ToTable("ticket_statuses");
        builder.HasKey(s => s.TicketId);
        builder.HasOne(s => s.Ticket)
            .WithMany()
            .HasForeignKey(s => s.TicketId)
            .IsRequired();
        builder.HasOne(s => s.Purchaser)
            .WithMany()
            .HasForeignKey(s => s.PurchasedById)
            .IsRequired();
    }
}
