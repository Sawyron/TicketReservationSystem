using TicketReservation.Persistence.TicketTypes;

namespace TicketReservation.Persistence.Tickets;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> FindAvalilableTicketsAsync(Guid trainId, Guid typeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TicketType>> GetAllTicketTypesAsync(CancellationToken cancellationToken = default);
}