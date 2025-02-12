using Sales.Domain.Abstractions;

namespace Sales.Domain.Sales;

public sealed record SaleCancelledDomainEvent(Guid Id, Guid SaledId) : DomainEvent(Id);