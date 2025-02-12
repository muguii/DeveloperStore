using AutoMapper;
using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;
using Sales.Domain.Sales;
using Sales.Domain.Shared;

namespace Sales.Application.Sales.Commands.AddItem;

internal sealed class AddSaleItemCommandHandler : IRequestHandler<AddSaleItemCommand, ApplicationResult<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddSaleItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleDto>> Handle(AddSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _unitOfWork.Sale.GetByIdWithProductsAsync(request.SaleId, cancellationToken).ConfigureAwait(false);
        if (sale is null)
            return ApplicationResult<SaleDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Sale), request.SaleId.ToString()));

        var product = await _unitOfWork.Product.GetByIdAsync(request.ProductId, cancellationToken).ConfigureAwait(false);
        if (product is null)
            return ApplicationResult<SaleDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Product), request.ProductId.ToString()));

        var saleItemResult = sale.AddItem(product!, request.Quantity);
        if (saleItemResult.Failed)
            return ApplicationResult<SaleDto>.BadRequest(saleItemResult.FailureDetails);

        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ApplicationResult<SaleDto>.Success(_mapper.Map<SaleDto>(sale));
    }
}