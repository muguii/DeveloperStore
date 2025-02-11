using Sales.Domain.Abstractions;
using Sales.Domain.Carts;
using Sales.Domain.Products;

namespace Sales.Application.Services;

public interface IUnitOfWork
{
    IRepository<Product> Product { get; }
    ICartRepository Cart { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}