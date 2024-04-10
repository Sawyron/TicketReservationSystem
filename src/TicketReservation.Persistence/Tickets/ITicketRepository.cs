namespace TicketReservation.Persistence.Tickets;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> FindAvalilableTicketsAsync(Guid trainId, Guid typeId, CancellationToken cancellationToken = default);
}