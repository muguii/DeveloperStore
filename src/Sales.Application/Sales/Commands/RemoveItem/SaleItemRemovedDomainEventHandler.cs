using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Domain.Sales;
using System.Diagnostics;
using System.Text.Json;

namespace Sales.Application.Sales.Commands.RemoveItem;

internal sealed class SaleItemRemovedDomainEventHandler : INotificationHandler<SaleItemRemovedDomainEvent>
{
    private readonly ILogger<SaleItemRemovedDomainEventHandler> _logger;

    public SaleItemRemovedDomainEventHandler(ILogger<SaleItemRemovedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleItemRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Publish to Message Broker

        var message = $"The Domain Event {nameof(SaleItemRemovedDomainEvent)} Ocurred: {JsonSerializer.Serialize(notification)}";
        Debug.WriteLine(message);
        _logger.LogInformation(message);

        return Task.CompletedTask;
    }
}