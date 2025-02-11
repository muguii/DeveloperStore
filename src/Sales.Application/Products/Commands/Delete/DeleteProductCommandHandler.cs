using AutoMapper;
using MediatR;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Application.Products.Commands.Delete;

internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApplicationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (product is null)
            return ApplicationResult.NotFound(FailureDetail.ResourceNotFound(nameof(Product), request.Id.ToString()));

        _unitOfWork.Product.Remove(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult.NoContent();
    }
}