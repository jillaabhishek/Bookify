using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("apartment");

            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Address);

            builder.Property(apartment => apartment.Name)
                   .HasMaxLength(100)
                   .HasConversion(name => name.name, value => new Name(value));

            builder.Property(apartment => apartment.Description)
                   .HasMaxLength(2000)
                   .HasConversion(description => description.description, value => new Description(value));

            builder.OwnsOne(apartment => apartment.Price, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                            .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(apartment => apartment.CleaningFee, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                            .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
