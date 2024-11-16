using TicketReservation.Persistence.TicketTypes;

namespace TicketReservation.Persistence.Tickets;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> FindAvailableTicketsAsync(Guid trainId, Guid typeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TicketType>> GetAllTicketTypesAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Ticket>> GetClientTicketsAsync(Guid clientId, CancellationToken cancellationToken = default);
}