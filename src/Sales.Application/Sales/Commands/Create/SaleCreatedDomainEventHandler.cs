using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Domain.Sales;
using System.Diagnostics;
using System.Text.Json;

namespace Sales.Application.Sales.Commands.Create;

internal sealed class SaleCreatedDomainEventHandler : INotificationHandler<SaleCreatedDomainEvent>
{
    private readonly ILogger<SaleCreatedDomainEventHandler> _logger;

    public SaleCreatedDomainEventHandler(ILogger<SaleCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Publish to Message Broker

        var message = $"The Domain Event {nameof(SaleCreatedDomainEvent)} Ocurred: {JsonSerializer.Serialize(notification)}";
        Debug.WriteLine(message);
        _logger.LogInformation(message);

        return Task.CompletedTask;
    }
}