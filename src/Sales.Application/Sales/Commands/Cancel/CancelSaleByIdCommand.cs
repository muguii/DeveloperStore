using MediatR;
using Sales.Application.Shared;

namespace Sales.Application.Sales.Commands.Cancel;

public sealed record CancelSaleByIdCommand : IRequest<ApplicationResult>
{
    public Guid Id { get; set; }
}