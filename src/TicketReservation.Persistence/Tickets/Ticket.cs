using TicketReservation.Persistence.TicketTypes;
using TicketReservation.Persistence.Trains;

namespace TicketReservation.Persistence.Tickets;

public class Ticket
{
    public Guid Id { get; set; }

    public int PlaceNumber { get; set; }

    public Guid TypeId { get; set; }

    public TicketType TicketType { get; set; } = default!;

    public Guid TrainId { get; set; }

    public Train Train { get; set; } = default!;
}
