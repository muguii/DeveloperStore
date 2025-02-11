using Microsoft.EntityFrameworkCore;
using Sales.Domain.Carts;

namespace Sales.Infrastructure.Persistence.Repositories;

internal sealed class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<Cart?> GetByIdWithProductsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Entity.Include(c => c.Products)
                           .SingleOrDefaultAsync(p => p.Id == id, cancellationToken)
                           .ConfigureAwait(false);
    }
}