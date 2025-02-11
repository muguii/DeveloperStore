using AutoMapper;
using MediatR;
using Sales.Application.Carts.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Carts;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Application.Carts.Commands.Update;

internal sealed class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ApplicationResult<CartDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartDto>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _unitOfWork.Cart.GetByIdWithProductsAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (cart is null)
            return ApplicationResult<CartDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Cart), request.Id.ToString()));

        cart.Update(request.UserId, request.Date);
        cart.RemoveAllItems();

        // TODO: Move logic to a DomainService
        var errors = new List<FailureDetail>();
        foreach (UpdateCartItem item in request.Products)
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

        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ApplicationResult<CartDto>.Success(_mapper.Map<CartDto>(cart));
    }
}