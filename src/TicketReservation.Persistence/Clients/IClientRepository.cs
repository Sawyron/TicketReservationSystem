
namespace TicketReservation.Persistence.Clients;
public interface IClientRepository
{
    Task<Client?> FindByCredentials(string username, string password, CancellationToken cancellationToken = default);
}
