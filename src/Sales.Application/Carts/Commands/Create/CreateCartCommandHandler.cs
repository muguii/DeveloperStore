using AutoMapper;
using MediatR;
using Sales.Application.Carts.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Carts;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Application.Carts.Commands.Create;

internal sealed class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ApplicationResult<CartDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartDto>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var cartResult = Cart.Create(request.UserId, request.Date);
        if (cartResult.Failed)
            return ApplicationResult<CartDto>.BadRequest(cartResult.FailureDetails);

        var cart = cartResult.Value!;

        var errors = new List<FailureDetail>();
        foreach (CreateCartItem item in request.Products)
        {
            // TODO: Go to the database only once
            var product = await _unitOfWork.Product.GetByIdAsync(item.ProductId, cancellationToken).ConfigureAwait(false); 
            if (product is null)
            {
                errors.Add(FailureDetail.ResourceNotFound(nameof(Product), item.ProductId.ToString()));
                continue;
            }

            var cartItemResult = cart.AddItem(product!, item.Quantity);
            if (cartItemResult.Failed)
                errors.AddRange(cartItemResult.FailureDetails);
        }

        if (errors.Count > 0)
            return ApplicationResult<CartDto>.BadRequest(errors);

        await _unitOfWork.Cart.AddAsync(cart, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ApplicationResult<CartDto>.Created(_mapper.Map<CartDto>(cart));
    }
}
