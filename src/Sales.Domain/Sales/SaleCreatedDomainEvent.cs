using Sales.Domain.Abstractions;

namespace Sales.Domain.Sales;

public sealed record SaleCreatedDomainEvent(Guid Id, Guid SaledId) : DomainEvent(Id);