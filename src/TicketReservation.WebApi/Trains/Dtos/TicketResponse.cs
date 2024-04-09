namespace TicketReservation.WebApi.Trains.Dtos;

public record TicketResponse(Guid Id, int PlaceNumber, string Type, bool IsPurchased);
