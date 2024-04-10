using Microsoft.EntityFrameworkCore;

namespace TicketReservation.Persistence.Clients;

internal sealed class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ClientRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Client?> FindByCredentials(string username, string password, CancellationToken cancellationToken = default) =>
        _dbContext.Set<Client>().FirstOrDefaultAsync(
            client => client.Name == username && client.Password == password,
            cancellationToken);
}
