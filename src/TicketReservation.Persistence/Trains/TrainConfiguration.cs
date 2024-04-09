using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketReservation.Persistence.Trains;
internal class TrainConfiguration : IEntityTypeConfiguration<Train>
{
    public void Configure(EntityTypeBuilder<Train> builder)
    {
        builder.ToTable("trains");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(100);
        builder.HasIndex(t => t.Name)
            .IsUnique();
        builder.HasMany(t => t.Tickets)
            .WithOne(t => t.Train)
            .HasForeignKey(t => t.TrainId)
            .IsRequired();
    }
}
