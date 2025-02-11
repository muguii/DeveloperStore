using AutoMapper;
using MediatR;
using Sales.Application.Carts.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Carts;
using Sales.Domain.Shared;

namespace Sales.Application.Carts.Queries.GetById;

internal sealed class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, ApplicationResult<CartDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCartByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartDto>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _unitOfWork.Cart.GetByIdWithProductsAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (cart is null)
            return ApplicationResult<CartDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Cart), request.Id.ToString()));

        return ApplicationResult<CartDto>.Created(_mapper.Map<CartDto>(cart));
    }
}