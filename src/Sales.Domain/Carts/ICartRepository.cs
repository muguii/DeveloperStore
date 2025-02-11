using Sales.Domain.Abstractions;

namespace Sales.Domain.Carts;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByIdWithProductsAsync(Guid id, CancellationToken cancellationToken);
}