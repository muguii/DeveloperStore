namespace Sales.Domain.Abstractions;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    IQueryable<TEntity> AsQueryable();
    Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void RemoveRange(List<TEntity> entities);
    void Remove(TEntity entity);
}