using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");

            builder.HasKey(review => review.Id);

            builder.Property(review => review.Rating)
                   .HasConversion(review => review.Value, value => Rating.Create(value).Value);

            builder.Property(review => review.Comment)
                   .HasMaxLength(200)
                   .HasConversion(review => review.Value, value => new Comment(value));

            builder.Property(review => review.CreatedOnUtc);

            builder.HasOne<Apartment>()
                   .WithMany()
                   .HasForeignKey(apartment => apartment.Id);

            builder.HasOne<Booking>()
                   .WithMany()
                   .HasForeignKey(booking => booking.Id);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(user => user.Id);
        }
    }
}
