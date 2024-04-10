using Microsoft.EntityFrameworkCore;
using TicketReservation.Persistence.TicketStatuses;

namespace TicketReservation.Persistence.Tickets;

internal class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Ticket>> FindAvalilableTicketsAsync(
        Guid trainId,
        Guid typeId,
        CancellationToken cancellationToken = default)
    {
        var tickets = await
            (from t in _dbContext.Set<Ticket>()
             join s in _dbContext.Set<TicketStatus>()
               on t.Id equals s.TicketId into grouping
             from s in grouping.DefaultIfEmpty()
             where t.TrainId == trainId
             where t.TypeId == typeId
             where s == null
             select t).ToListAsync(cancellationToken);
        return tickets;
    }
}
