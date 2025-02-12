using AutoMapper;
using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Sales;
using Sales.Domain.Shared;

namespace Sales.Application.Sales.Commands.Update;

internal sealed class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, ApplicationResult<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleDto>> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _unitOfWork.Sale.GetByIdWithProductsAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (sale is null)
            return ApplicationResult<SaleDto>.NotFound(FailureDetail.ResourceNotFound(nameof(Sale), request.Id.ToString()));

        sale.Update(request.Customer, request.Branch);

        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ApplicationResult<SaleDto>.Success(_mapper.Map<SaleDto>(sale));
    }
}