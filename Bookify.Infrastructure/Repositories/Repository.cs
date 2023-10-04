using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Repositories
{
    internal abstract class Repository<T> where T : Entity
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<T>()
                                  .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        } 

        public void Add(T entity) {
            DbContext.Add(entity);
        }
    }
}
