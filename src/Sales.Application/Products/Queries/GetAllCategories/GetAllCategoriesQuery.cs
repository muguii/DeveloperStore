using MediatR;
using Sales.Application.Shared;

namespace Sales.Application.Products.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<ApplicationResult<List<string>>>
{

}