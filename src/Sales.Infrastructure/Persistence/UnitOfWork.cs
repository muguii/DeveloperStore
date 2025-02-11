using Sales.Application.Services;
using Sales.Domain.Abstractions;
using Sales.Domain.Products;

namespace Sales.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public IRepository<Product> Product { get; }

    public UnitOfWork(ApplicationDbContext dbContext, IRepository<Product> product)
    {
        _dbContext = dbContext;
        
        Product = product;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) 
        => await _dbContext.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_dbContext.Database.CurrentTransaction is not null)
                await _dbContext.Database.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await RollbackAsync(cancellationToken);

            throw;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.CurrentTransaction is not null)
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}