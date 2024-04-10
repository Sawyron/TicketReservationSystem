using Microsoft.EntityFrameworkCore;

namespace TicketReservation.Persistence.Trains;

internal class TrainRepository : ITrainRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TrainRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Train?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Train>()
            .Include(t => t.Tickets.OrderBy(t => t.PlaceNumber))
            .ThenInclude(t => t.TicketType)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Train>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Train>()
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }
}
