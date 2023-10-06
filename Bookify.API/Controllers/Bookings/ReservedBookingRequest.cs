namespace Bookify.API.Controllers.Bookings;

public sealed record ReservedBookingRequest(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);

