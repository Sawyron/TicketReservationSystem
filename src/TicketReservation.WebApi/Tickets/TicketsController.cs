using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketReservation.Persistence.Tickets;
using TicketReservation.Persistence.TicketStatuses;
using TicketReservation.WebApi.Tickets.Dtos;
using TicketReservation.WebApi.Trains.Dtos;

namespace TicketReservation.WebApi.Tickets;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class TicketsController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketStatusRepository _ticketStatusRepository;

    public TicketsController(ITicketRepository ticketRepository, ITicketStatusRepository ticketStatusRepository)
    {
        _ticketRepository = ticketRepository;
        _ticketStatusRepository = ticketStatusRepository;
    }

    [HttpGet("types")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TicketTypeResponse>))]
    public async Task<IActionResult> GetAllTypes(CancellationToken cancellationToken)
    {
        var types = await _ticketRepository.GetAllTicketTypesAsync(cancellationToken);
        var response = types.Select(t => new TicketTypeResponse(t.Id, t.Name))
            .ToList();
        return Ok(types);
    }

    [HttpGet("free")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TicketResponse>))]
    public async Task<IActionResult> GetFreeSpaces(
        [FromQuery] FreeSpacesQuery query, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.FindAvalilableTicketsAsync(
            query.TrainId,
            query.TypeId,
            cancellationToken);
        var response = tickets.Select(t =>
            new TicketResponse(t.Id, t.PlaceNumber, string.Empty, false)).ToList();
        return Ok(response);
    }

    [HttpGet("my-tickets")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ClientTicketResponse>))]
    public async Task<IActionResult> GetClientTickets(CancellationToken cancellationToken)
    {
        Guid clientId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var tickets = await _ticketRepository.GetClinetTicketsAsync(clientId, cancellationToken);
        var response = tickets.Select(t =>
            new ClientTicketResponse(
                t.Id,
                t.PlaceNumber,
                t.TrainId,
                t.Train.Name,
                t.TypeId,
                t.TicketType.Name))
            .ToList();
        return Ok(response);
    }

    [HttpPost("purchase")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PurchaseTicket(
        [FromBody] PurchaseTicketRequest request,
        CancellationToken cancellationToken = default)
    {
        var clientId = Guid.Parse(HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
        bool result = await _ticketStatusRepository.AddPurchasedStatus(
            request.TicketId,
            clientId,
            cancellationToken);
        return result ?
            Ok() :
            BadRequest(new ProblemDetails
            {
                Title = "Bad request",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Ticket with id '{request.TicketId}' is already purchased.",
                Instance = HttpContext.TraceIdentifier,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            });
    }
}
