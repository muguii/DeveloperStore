using MediatR;
using Sales.Application.Carts.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Carts.Queries.GetAll;

public sealed record GetAllCartsQuery : PagingFilter, IRequest<ApplicationResult<Paging<CartDto>>>
{
    public DateTime? MinDate { get; init; }
    public DateTime? MaxDate { get; init; }
    public string? OrderBy { get; init; }
}