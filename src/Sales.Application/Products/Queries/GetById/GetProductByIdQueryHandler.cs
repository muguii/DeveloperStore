using AutoMapper;
using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Application.Products.Queries.GetById;

internal sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApplicationResult<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (product is null)
            return ApplicationResult<ProductDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Product), request.Id.ToString()));

        return ApplicationResult<ProductDto>.Created(_mapper.Map<ProductDto>(product));
    }
}