using AutoMapper;
using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Sales;
using Sales.Domain.Shared;

namespace Sales.Application.Sales.Queries.GetById;

internal sealed class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, ApplicationResult<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSaleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleDto>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _unitOfWork.Sale.GetByIdWithProductsAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (sale is null)
            return ApplicationResult<SaleDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Sale), request.Id.ToString()));

        return ApplicationResult<SaleDto>.Created(_mapper.Map<SaleDto>(sale));
    }
}