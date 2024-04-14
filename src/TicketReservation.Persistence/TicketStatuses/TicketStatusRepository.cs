using Microsoft.EntityFrameworkCore;

namespace TicketReservation.Persistence.TicketStatuses;

internal class TicketStatusRepository : ITicketStatusRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketStatusRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddPurchasedStatus(Guid ticketId, Guid purchaserId, CancellationToken cancellationToken = default)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        var existing = await _dbContext.Set<TicketStatus>()
            .FirstOrDefaultAsync(s => s.TicketId == ticketId, cancellationToken);
        if (existing is not null)
        {
            return false;
        }
        var status = new TicketStatus
        {
            PurchasedById = purchaserId,
            TicketId = ticketId,
            PuchasedOn = DateTime.UtcNow
        };
        await _dbContext.AddAsync(status, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<TicketStatus>> FindTicketStatusesForTrain(Guid trainId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TicketStatus>()
            .Include(t => t.Ticket)
            .ThenInclude(t => t.Train)
            .Where(t => t.Ticket.TrainId == trainId)
            .ToListAsync(cancellationToken);
    }
}
