using MediatR;
using Sales.Application.Services;
using Sales.Domain.Abstractions;
using Sales.Domain.Carts;
using Sales.Domain.Products;
using Sales.Domain.Sales;

namespace Sales.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public IRepository<Product> Product { get; }
    public ICartRepository Cart { get; }
    public ISaleRepository Sale { get; }

    public UnitOfWork(ApplicationDbContext dbContext,
                      IPublisher publisher,
                      IRepository<Product> product,
                      ICartRepository cart,
                      ISaleRepository sale)
    {
        _dbContext = dbContext;

        Product = product;
        Cart = cart;
        Sale = sale;
        _publisher = publisher;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            // The most correct thing would be to implement the Outbox Pattern to guarantee the atomicity of the operation
            await PublishDomainEventsAsync();
        }
        catch (Exception)
        {
            // Log...
        }

        return result;
    }

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

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = _dbContext.ChangeTracker.Entries<BaseEntity>()
                                                   .Select(e => e.Entity)
                                                   .Where(e => e.DomainEvents.Count > 0)
                                                   .SelectMany(e =>
                                                   {
                                                       var domainEvents = e.DomainEvents;
                                                       e.ClearDomainEvents();
                                                       return domainEvents;
                                                   })
                                                   .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
}