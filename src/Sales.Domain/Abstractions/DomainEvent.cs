using MediatR;

namespace Sales.Domain.Abstractions;

public record DomainEvent(Guid Id) : INotification;