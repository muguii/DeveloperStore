using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Products.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;
using System.Linq.Dynamic.Core;

namespace Sales.Application.Products.Queries.GetAll;

internal sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ApplicationResult<Paging<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<Paging<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await BuildWhereQuery(request)
                             .OrderBy(QueryUtils.RemoveWrongOrderByProperties<Product>(request.OrderBy))
                             .Select(p => _mapper.Map<ProductDto>(p))
                             .ToPagedListAsync(request, cancellationToken)
                             .ConfigureAwait(false);

        return ApplicationResult<Paging<ProductDto>>.Success(products);
    }

    private IQueryable<Product> BuildWhereQuery(GetAllProductsQuery request)
    {
        var query = _unitOfWork.Product.AsQueryable();

        if (request.Title?.Count > 0)
            query = query.Where(product => QueryUtils.ConvertToSqlLikePattern(request.Title).Any(title => EF.Functions.ILike(product.Title, title)));

        if (request.Description?.Count > 0)
            query = query.Where(product => QueryUtils.ConvertToSqlLikePattern(request.Description).Any(description => EF.Functions.ILike(product.Description, description)));

        if (request.Category?.Count > 0)
            query = query.Where(product => QueryUtils.ConvertToSqlLikePattern(request.Category).Any(category => EF.Functions.ILike(product.Category, category)));

        if (request.Images?.Count > 0)
            query = query.Where(product => QueryUtils.ConvertToSqlLikePattern(request.Images).Any(image => EF.Functions.ILike(product.Image, image)));

        if (request.MinPrice.HasValue)
            query = query.Where(c => c.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(c => c.Price <= request.MaxPrice.Value);

        return query;
    }
}