namespace TicketReservation.WebApi.Tickets.Dtos;

public record ClientTicketResponse(
    Guid Id,
    int PlaceNumber,
    Guid TrainId,
    string TrainName,
    Guid TypeId,
    string TypeName);
