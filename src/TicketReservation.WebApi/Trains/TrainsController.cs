using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketReservation.Persistence.TicketStatuses;
using TicketReservation.Persistence.Trains;
using TicketReservation.WebApi.Trains.Dtos;

namespace TicketReservation.WebApi.Trains;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class TrainsController : ControllerBase
{
    private readonly ITrainRepository _trainRepository;
    private readonly ITicketStatusRepository _ticketStatusRepository;

    public TrainsController(
        ITrainRepository trainRepository,
        ITicketStatusRepository ticketStatusRepository)
    {
        _trainRepository = trainRepository;
        _ticketStatusRepository = ticketStatusRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrainResponse>))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var trains = await _trainRepository.GetAllAsync(cancellationToken);
        var response = trains.Select(t => new TrainResponse(t.Id, t.Name)).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrainTicketsResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTrainTickets([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var train = await _trainRepository.FindByIdAsync(id, cancellationToken);
        if (train is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = $"Train is not found.",
                Detail = $"Train with id '{id}' is not found.",
                Instance = HttpContext.TraceIdentifier,
                Status = StatusCodes.Status404NotFound,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
            });
        }
        var statuses = (await _ticketStatusRepository.FindTicketStatusesForTrain(id, cancellationToken))
            .Select(s => s.TicketId)
            .ToHashSet();
        return Ok(new TrainTicketsResponse(
            train.Id,
            train.Name,
            train.Tickets.Select(t =>
                new TicketResponse(
                    t.Id,
                    t.PlaceNumber,
                    t.TicketType.Name,
                    statuses.Contains(t.Id)))
            .ToList()));
    }
}
