using TicketReservation.Persistence.Clients;
using TicketReservation.Persistence.Tickets;

namespace TicketReservation.Persistence.TicketStatuses;

public class TicketStatus
{
    public Guid TicketId { get; set; }

    public Ticket Ticket { get; set; } = default!;

    public Guid PurchasedById { get; set; }

    public Client Purchaser { get; set; } = default!;

    public DateTime PuchasedOn { get; set; }
}
