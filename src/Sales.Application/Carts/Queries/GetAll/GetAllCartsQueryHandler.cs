using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Carts.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Carts;
using System.Linq.Dynamic.Core;

namespace Sales.Application.Carts.Queries.GetAll;

internal sealed class GetAllCartsQueryHandler : IRequestHandler<GetAllCartsQuery, ApplicationResult<Paging<CartDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCartsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<Paging<CartDto>>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
    {
        var carts = await BuildWhereQuery(request)
                          .Include(c => c.Products)
                          .OrderBy(QueryUtils.RemoveWrongOrderByProperties<Cart>(request.OrderBy))
                          .Select(c => _mapper.Map<CartDto>(c))
                          .ToPagedListAsync(request, cancellationToken)
                          .ConfigureAwait(false);

        return ApplicationResult<Paging<CartDto>>.Success(carts);
    }

    private IQueryable<Cart> BuildWhereQuery(GetAllCartsQuery request)
    {
        var query = _unitOfWork.Cart.AsQueryable();

        if (request.MinDate.HasValue)
            query = query.Where(c => c.Date >= request.MinDate.Value);

        if (request.MaxDate.HasValue)
            query = query.Where(c => c.Date <= request.MaxDate.Value);

        return query;
    }
}