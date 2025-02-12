using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Domain.Sales;
using System.Diagnostics;
using System.Text.Json;

namespace Sales.Application.Sales.Commands.Update;

internal sealed class SaleModifiedDomainEventHandler : INotificationHandler<SaleModifiedDomainEvent>
{
    private readonly ILogger<SaleModifiedDomainEventHandler> _logger;

    public SaleModifiedDomainEventHandler(ILogger<SaleModifiedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleModifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Publish to Message Broker

        var message = $"The Domain Event {nameof(SaleModifiedDomainEvent)} Ocurred: {JsonSerializer.Serialize(notification)}";
        Debug.WriteLine(message);
        _logger.LogInformation(message);

        return Task.CompletedTask;
    }
}