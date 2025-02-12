using Sales.Domain.Abstractions;

namespace Sales.Domain.Sales;

public sealed record SaleItemRemovedDomainEvent(Guid Id, Guid SaledId) : DomainEvent(Id);