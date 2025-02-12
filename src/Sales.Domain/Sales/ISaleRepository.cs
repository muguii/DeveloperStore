using Sales.Domain.Abstractions;

namespace Sales.Domain.Sales;

public interface ISaleRepository : IRepository<Sale>
{
    Task<Sale?> GetByIdWithProductsAsync(Guid id, CancellationToken cancellationToken);
}