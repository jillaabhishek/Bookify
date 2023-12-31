﻿using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Apartments.SearchApartments
{
    internal sealed class SearchApartmentQueryHandler : IQueryHandler<SearchApartmentQuery, IReadOnlyList<ApartmentResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        //use Postgres SQL array is allow as a column. But in SQlServer it not,
        private static readonly int[] ActiveBookingStatuses =
        {
            (int)BookingStatus.Reserved,
            (int)BookingStatus.Confirmed,
            (int)BookingStatus.Completed,
        };       
        

        public SearchApartmentQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentQuery request, CancellationToken cancellationToken)
        {
            //That why we converting int[] to string and split the value with ","
            var activeBookingStatuses = string.Join(",", ActiveBookingStatuses.Select(x => x.ToString()).ToList());

            if (request.StartDate > request.EndDate)
                return Result.Success<IReadOnlyList<ApartmentResponse>>(new List<ApartmentResponse>());

            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                a.id AS Id,
                a.name AS Name,
                a.description AS Description,
                a.price_amount AS Price,
                a.price_currency AS Currency,
                a.address_country AS Country,
                a.address_state AS State,
                a.address_zip_code AS ZipCode,
                a.address_city AS City,
                a.address_street AS Street
            FROM apartments AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings AS b
                WHERE
                    b.apartment_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end >= @StartDate AND
                    b.status IN @ActiveBookingStatuses
            )
            """;

            var apartments = await connection.QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
                sql,
                (apartment, address) =>
                {
                    apartment.Address = address;
                    return apartment;
                },
                new
                {
                    request.StartDate,
                    request.EndDate,
                    ActiveBookingStatuses
                },
                splitOn: "Country");

            return Result.Success<IReadOnlyList<ApartmentResponse>>(apartments.ToList());
        }
    }
}
