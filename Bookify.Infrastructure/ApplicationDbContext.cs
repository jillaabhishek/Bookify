using Bookify.Application.Abstractions.Clock;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;
        private readonly IDateTimeProvider _dateProvider;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public ApplicationDbContext(
            DbContextOptions options,
            IPublisher publisher,
            IDateTimeProvider dateProvider) : base(options)
        {
            _publisher = publisher;
            _dateProvider = dateProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                AddDomainEventsAsOutboxMessages();

                var result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DBConcurrencyException ex)
            {
                throw new Application.Exceptions.ConcurrencyException("Concurrency exception occurred", ex);
            }
        }

        private void AddDomainEventsAsOutboxMessages()
        {
            var domainEvents = ChangeTracker
                                .Entries<IEntity>()
                                .Select(entry => entry.Entity)
                                .SelectMany(entity =>
                                    {
                                        var domainEvents = entity.GetDomainEvents();
                                        entity.ClearDomainEvents();

                                        return domainEvents;
                                    })
                                .Select(domainEvent => new OutboxMessage(
                                    Guid.NewGuid(),
                                    _dateProvider.UtcNow,
                                    domainEvent.GetType().ToString(),
                                    JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)
                                    ))
                            .ToList();

            AddRange(domainEvents);
        }
    }
}
