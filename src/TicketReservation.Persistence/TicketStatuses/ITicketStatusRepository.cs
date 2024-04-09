namespace TicketReservation.Persistence.TicketStatuses;

public interface ITicketStatusRepository
{
    Task<IEnumerable<TicketStatus>> FindTicketStatusesForTrain(Guid trainId, CancellationToken cancellationToken = default);

    Task<bool> AddPurchasedStatus(Guid ticketId, Guid purchaserId, CancellationToken cancellationToken = default);
}