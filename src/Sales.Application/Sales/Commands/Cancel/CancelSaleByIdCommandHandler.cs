using AutoMapper;
using MediatR;
using Sales.Application.Services;
using Sales.Application.Shared;
using Sales.Domain.Sales;
using Sales.Domain.Shared;

namespace Sales.Application.Sales.Commands.Cancel;

internal sealed class CancelSaleByIdCommandHandler : IRequestHandler<CancelSaleByIdCommand, ApplicationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancelSaleByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApplicationResult> Handle(CancelSaleByIdCommand request, CancellationToken cancellationToken)
    {
        var sale = await _unitOfWork.Sale.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (sale is null)
            return ApplicationResult.NotFound(FailureDetail.ResourceNotFound(nameof(Sale), request.Id.ToString()));

        var cancelResult = sale.Cancel();
        if (cancelResult.Failed)
            return ApplicationResult.BadRequest(cancelResult.FailureDetails);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult.Success();
    }
}