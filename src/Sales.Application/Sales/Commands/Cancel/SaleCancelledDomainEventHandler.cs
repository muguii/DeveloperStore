using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Domain.Sales;
using System.Diagnostics;
using System.Text.Json;

namespace Sales.Application.Sales.Commands.Cancel;

internal sealed class SaleCancelledDomainEventHandler : INotificationHandler<SaleCancelledDomainEvent>
{
    private readonly ILogger<SaleCancelledDomainEventHandler> _logger;

    public SaleCancelledDomainEventHandler(ILogger<SaleCancelledDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        // Publish to Message Broker

        var message = $"The Domain Event {nameof(SaleCancelledDomainEvent)} Ocurred: {JsonSerializer.Serialize(notification)}";
        Debug.WriteLine(message);
        _logger.LogInformation(message);

        return Task.CompletedTask;
    }
}