using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketReservation.Persistence.Clients;
using TicketReservation.Persistence.Tickets;
using TicketReservation.Persistence.TicketStatuses;
using TicketReservation.Persistence.TicketTypes;
using TicketReservation.Persistence.Trains;

namespace TicketReservation.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseSqlite(configuration.GetConnectionString("Sqlite"))
                    .UseSnakeCaseNamingConvention())
            .AddScoped<IClientRepository, ClientRepository>()
            .AddScoped<ITrainRepository, TrainRepository>()
            .AddScoped<ITicketStatusRepository, TicketStatusRepository>()
            .AddScoped<ITicketRepository, TicketRepository>();

    public static bool EnsureDbCreated(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return context.Database.EnsureCreated();
    }

    public static void PolulateDb(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var clients = new List<Client>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Tur",
                Password = "12345"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Ovidius",
                Password = "qwerty"
            }
        };
        var ticketTypes = new List<TicketType>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Platskart"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Compartment"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Lixury"
            }
        };
        var trains = new List<Train>();
        foreach (char c in "ABCDEF")
        {
            var tickets = Enumerable.Range(0, 10)
                .Select(i => new Ticket
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = i + 1,
                    TicketType = ticketTypes[2],
                })
                .ToList();
            tickets.AddRange(Enumerable.Range(10, 20)
                .Select(i => new Ticket
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = i + 1,
                    TicketType = ticketTypes[1]
                }));
            tickets.AddRange(Enumerable.Range(30, 30)
                .Select(i => new Ticket
                {
                    Id = Guid.NewGuid(),
                    PlaceNumber = i + 1,
                    TicketType = ticketTypes[0]
                }));
            trains.Add(new Train
            {
                Id = Guid.NewGuid(),
                Name = c.ToString(),
                Tickets = tickets
            });
        }
        context.AddRange(clients);
        context.AddRange(ticketTypes);
        context.AddRange(trains);
        context.SaveChanges();
    }
}
