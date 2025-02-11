using Microsoft.EntityFrameworkCore;
using Sales.Domain.Abstractions;

namespace Sales.Infrastructure.Persistence.Repositories;

internal class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected DbSet<TEntity> Entity { get; init; }

    public BaseRepository(ApplicationDbContext dbContext)
    {
        Entity = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await Entity.SingleOrDefaultAsync(e => e.Id == id, cancellationToken).ConfigureAwait(false);

    public IQueryable<TEntity> AsQueryable()
        => Entity;

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => await Entity.AddAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
        => await Entity.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

    public void Remove(TEntity entity)
        => Entity.Remove(entity);

    public void RemoveRange(List<TEntity> entities)
        => Entity.RemoveRange(entities);
}