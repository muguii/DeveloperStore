using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Sales.Commands.AddItem;
using Sales.Application.Sales.Commands.Cancel;
using Sales.Application.Sales.Commands.Create;
using Sales.Application.Sales.Commands.RemoveItem;
using Sales.Application.Sales.Commands.Update;
using Sales.Application.Sales.Queries.GetById;
using Sales.Application.Sales.Shared;
using Sales.Application.Shared;

namespace Sales.API.Sales;

[Route("api/sales")]
public sealed class SaleController : Shared.ControllerBase
{
    public SaleController(ISender sender) : base(sender)
    {

    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetSaleById(Guid id)
    {
        ApplicationResult<SaleDto> getSaleByIdResult = await Sender.Send(new GetSaleByIdQuery { Id = id }).ConfigureAwait(false);
        return ApplicationResultToActionResult(getSaleByIdResult, x => x.Value!);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand createSaleCommand)
    {
        ApplicationResult<SaleDto> createSaleResult = await Sender.Send(createSaleCommand).ConfigureAwait(false);
        return ApplicationResultToActionResult(createSaleResult, x => x.Value);
    }

    [HttpPost("{id:Guid}/add-item")]
    public async Task<IActionResult> AddItem(Guid id, [FromBody] AddSaleItemCommand addSaleItemCommand)
    {
        addSaleItemCommand.SaleId = id;
        ApplicationResult<SaleDto> addSaleItemResult = await Sender.Send(addSaleItemCommand).ConfigureAwait(false);
        return ApplicationResultToActionResult(addSaleItemResult, x => x.Value);
    }

    [HttpPost("{id:Guid}/remove-item")]
    public async Task<IActionResult> RemoveItem(Guid id, [FromBody] RemoveSaleItemCommand removeSaleItemCommand)
    {
        removeSaleItemCommand.SaleId = id;
        ApplicationResult<SaleDto> removeSaleItemResult = await Sender.Send(removeSaleItemCommand).ConfigureAwait(false);
        return ApplicationResultToActionResult(removeSaleItemResult, x => x.Value);
    }

    [HttpPost("{id:Guid}/cancel")]
    public async Task<IActionResult> CancelById(Guid id)
    {
        ApplicationResult cancelSaleResult = await Sender.Send(new CancelSaleByIdCommand { Id = id }).ConfigureAwait(false);
        return ApplicationResultToActionResult(cancelSaleResult);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand updateSaleCommand)
    {
        updateSaleCommand.Id = id;
        ApplicationResult<SaleDto> updateSaleResult = await Sender.Send(updateSaleCommand).ConfigureAwait(false);
        return ApplicationResultToActionResult(updateSaleResult, x => x.Value);
    }
}