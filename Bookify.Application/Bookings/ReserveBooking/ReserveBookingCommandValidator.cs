using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Bookings.ReserveBooking
{
    public class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ApartmentId).NotEmpty();
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
        }
    }
}
