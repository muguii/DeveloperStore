using AutoMapper;
using MediatR;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Carts;
using Sales.Domain.Shared;

namespace Sales.Application.Products.Commands.DeleteById;

internal sealed class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, ApplicationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteProductByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (product is null)
            return ApplicationResult.NotFound(FailureDetail.ResourceNotFound(nameof(Cart), request.Id.ToString()));

        _unitOfWork.Product.Remove(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult.NoContent();
    }
}