using MediatR;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Carts;
using Sales.Domain.Shared;

namespace Sales.Application.Carts.Commands.DeleteById;

internal sealed class DeleteCartByIdCommandHandler : IRequestHandler<DeleteCartByIdCommand, ApplicationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCartByIdCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult> Handle(DeleteCartByIdCommand request, CancellationToken cancellationToken)
    {
        var cart = await _unitOfWork.Cart.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (cart is null)
            return ApplicationResult.NotFound(FailureDetail.ResourceNotFound(nameof(Cart), request.Id.ToString()));

        _unitOfWork.Cart.Remove(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult.NoContent();
    }
}