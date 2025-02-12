using Sales.Domain.Abstractions;

namespace Sales.Domain.Sales;

public sealed record SaleModifiedDomainEvent(Guid Id, Guid SaledId) : DomainEvent(Id);