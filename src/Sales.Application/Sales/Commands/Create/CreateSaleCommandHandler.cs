using AutoMapper;
using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Products;
using Sales.Domain.Sales;
using Sales.Domain.Shared;

namespace Sales.Application.Sales.Commands.Create;

internal class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, ApplicationResult<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleDto>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var saleResult = Sale.Create(request.Customer, request.Branch);
        if (saleResult.Failed)
            return ApplicationResult<SaleDto>.BadRequest(saleResult.FailureDetails);

        var sale = saleResult.Value!;

        var errors = new List<FailureDetail>();
        foreach (CreateSaleItem item in request.Products)
        {
            // TODO: Go to the database only once
            var product = await _unitOfWork.Product.GetByIdAsync(item.ProductId, cancellationToken).ConfigureAwait(false);
            if (product is null)
            {
                errors.Add(FailureDetail.ResourceNotFound(nameof(Product), item.ProductId.ToString()));
                continue;
            }

            var saleItemResult = sale.AddItem(product!, item.Quantity);
            if (saleItemResult.Failed)
                errors.AddRange(saleItemResult.FailureDetails);
        }

        if (errors.Count > 0)
            return ApplicationResult<SaleDto>.BadRequest(errors);

        await _unitOfWork.Sale.AddAsync(sale, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ApplicationResult<SaleDto>.Created(_mapper.Map<SaleDto>(sale));
    }
}