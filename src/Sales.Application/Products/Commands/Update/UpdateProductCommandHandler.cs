using AutoMapper;
using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Application.Products.Commands.Update;

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApplicationResult<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (product is null)
            return ApplicationResult<ProductDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Product), request.Id.ToString()));

        product.Update(request.Title, request.Price, request.Description, request.Category, request.Image, request.Rating.Rate, request.Rating.Count);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<ProductDto>.Created(_mapper.Map<ProductDto>(product));
    }
}