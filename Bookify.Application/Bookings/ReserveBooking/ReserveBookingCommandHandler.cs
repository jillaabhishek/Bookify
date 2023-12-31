﻿using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Bookings.ReserveBooking
{
    public sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IUserRepository _userRespository;
        private readonly IApartmentRepository _aparmentRespository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PricingService _pricingService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ReserveBookingCommandHandler(
            IUserRepository userRespository,
            IApartmentRepository aparmentRespository,
            IUnitOfWork unitOfWork,
            PricingService pricingService,
            IBookingRepository bookingRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _userRespository = userRespository;
            _aparmentRespository = aparmentRespository;
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _bookingRepository = bookingRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetByIdAsync(new UserId(request.UserId));
            if (user is null) return Result.Failure<Guid>(UserErrors.NotFound);

            var apartment = await _aparmentRespository.GetByIdAsync(new ApartmentId(request.ApartmentId));
            if (apartment is null) return Result.Failure<Guid>(ApartmentErrors.NotFound);

            var duration = DateRange.Create(request.StartDate, request.EndDate);

            if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
                return Result.Failure<Guid>(BookingErrors.Overlap);

            try
            {
                var booking = Booking.Reserve(apartment, new UserId(request.UserId), duration, _dateTimeProvider.UtcNow, _pricingService);

                _bookingRepository.Add(booking);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success<Guid>(booking.Id.Value);
            }
            catch (ConcurrencyException ex)
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }
        }
    }
}
