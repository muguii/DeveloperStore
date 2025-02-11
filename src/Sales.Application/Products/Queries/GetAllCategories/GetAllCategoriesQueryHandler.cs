using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Services;
using Sales.Application.Shared;

namespace Sales.Application.Products.Queries.GetAllCategories;

internal sealed class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, ApplicationResult<List<string>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<List<string>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.Product.AsQueryable()
                                                  .AsNoTracking()
                                                  .Select(p => p.Category)
                                                  .Distinct()
                                                  .ToListAsync(cancellationToken);

        return ApplicationResult<List<string>>.Success(categories);
    }
}