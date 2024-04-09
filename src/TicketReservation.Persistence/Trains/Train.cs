using TicketReservation.Persistence.Tickets;

namespace TicketReservation.Persistence.Trains;
public class Train
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Ticket> Tickets { get; set; } = [];
}
