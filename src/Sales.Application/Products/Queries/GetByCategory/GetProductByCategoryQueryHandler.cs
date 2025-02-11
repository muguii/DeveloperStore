using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Products.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;
using System.Linq.Dynamic.Core;

namespace Sales.Application.Products.Queries.GetByCategory;

internal sealed class GetProductByCategoryQueryHandler : IRequestHandler<GetProductByCategoryQuery, ApplicationResult<Paging<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<Paging<ProductDto>>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await BuildWhereQuery(request)
                             .OrderBy(QueryUtils.RemoveWrongOrderByProperties<Product>(request.OrderBy))
                             .Select(p => _mapper.Map<ProductDto>(p))
                             .ToPagedListAsync(request, cancellationToken)
                             .ConfigureAwait(false);

        return ApplicationResult<Paging<ProductDto>>.Success(products);
    }

    private IQueryable<Product> BuildWhereQuery(GetProductByCategoryQuery request)
    {
        var query = _unitOfWork.Product.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Category))
            query = query.Where(p => EF.Functions.ILike(p.Category, request.Category));

        return query;
    }
}