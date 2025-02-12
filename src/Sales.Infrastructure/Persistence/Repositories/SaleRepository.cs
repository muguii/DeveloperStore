using Microsoft.EntityFrameworkCore;
using Sales.Domain.Sales;

namespace Sales.Infrastructure.Persistence.Repositories;

internal sealed class SaleRepository : BaseRepository<Sale>, ISaleRepository
{
    public SaleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<Sale?> GetByIdWithProductsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Entity.Include(s => s.Products)
                           .SingleOrDefaultAsync(s => s.Id == id, cancellationToken)
                           .ConfigureAwait(false);
    }
}