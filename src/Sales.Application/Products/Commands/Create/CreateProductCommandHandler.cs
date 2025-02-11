using AutoMapper;
using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;

namespace Sales.Application.Products.Commands.Create;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApplicationResult<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var ratingResult = Rating.Create(request.Rating.Rate, request.Rating.Count);
        if (ratingResult.Failed)
            return ApplicationResult<ProductDto>.BadRequest(ratingResult.FailureDetails);

        var productResult = Product.Create(request.Title, request.Price, request.Description, request.Category, request.Image, ratingResult.Value!);
        if (productResult.Failed)
            return ApplicationResult<ProductDto>.BadRequest(productResult.FailureDetails);

        await _unitOfWork.Product.AddAsync(productResult.Value!, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ApplicationResult<ProductDto>.Created(_mapper.Map<ProductDto>(productResult.Value!));
    }
}