using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketReservation.Persistence.Clients;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(c => c.Password)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(c => c.Name)
            .IsUnique();
    }

    private static IEnumerable<Client> GetClients()
    {
        yield return new Client
        {
            Id = Guid.NewGuid(),
            Name = "Tur",
            Password = "12345"
        };
        yield return new Client
        {
            Id = Guid.NewGuid(),
            Name = "Ovidius",
            Password = "qwerty"
        };
    }
}
