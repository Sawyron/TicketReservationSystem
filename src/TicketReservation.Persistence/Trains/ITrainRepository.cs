namespace TicketReservation.Persistence.Trains;
public interface ITrainRepository
{
    Task<IEnumerable<Train>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Train?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
