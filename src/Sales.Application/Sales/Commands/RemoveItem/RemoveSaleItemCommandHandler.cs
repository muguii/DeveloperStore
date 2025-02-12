using AutoMapper;
using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Sales;
using Sales.Domain.Shared;

namespace Sales.Application.Sales.Commands.RemoveItem;

internal sealed class RemoveSaleItemCommandHandler : IRequestHandler<RemoveSaleItemCommand, ApplicationResult<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RemoveSaleItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleDto>> Handle(RemoveSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _unitOfWork.Sale.GetByIdWithProductsAsync(request.SaleId, cancellationToken).ConfigureAwait(false);
        if (sale is null)
            return ApplicationResult<SaleDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Sale), request.SaleId.ToString()));

        var saleItemResult = sale.RemoveItemById(request.ItemId);
        if (saleItemResult.Failed)
            return ApplicationResult<SaleDto>.BadRequest(saleItemResult.FailureDetails);

        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ApplicationResult<SaleDto>.Success(_mapper.Map<SaleDto>(sale));
    }
}