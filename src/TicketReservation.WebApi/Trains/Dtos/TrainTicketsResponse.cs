namespace TicketReservation.WebApi.Trains.Dtos;

public record TrainTicketsResponse(
    Guid TrainId,
    string TrainName,
    List<TicketResponse> Tickets);
