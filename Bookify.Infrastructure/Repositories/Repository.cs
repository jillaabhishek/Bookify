using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Repositories
{
    internal abstract class Repository<TEntity, TEntityId> 
        where TEntity : Entity<TEntityId>
        where TEntityId : class
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<TEntity>()
                                  .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public void Add(TEntity entity)
        {
            DbContext.Add(entity);
        }
    }
}
