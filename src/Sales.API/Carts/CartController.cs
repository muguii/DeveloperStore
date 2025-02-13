using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Carts.Commands.Create;
using Sales.Application.Carts.Commands.DeleteById;
using Sales.Application.Carts.Commands.Update;
using Sales.Application.Carts.Queries.GetAll;
using Sales.Application.Carts.Queries.GetById;
using Sales.Application.Carts.Shared;
using Sales.Application.Shared;

namespace Sales.API.Carts;

[Route("carts")]
public sealed class CartController : Shared.ControllerBase
{
    public CartController(ISender sender) : base(sender)
    {

    }

    [HttpGet]
    public async Task<IActionResult> GetAllCarts([FromQuery] GetAllCartsQuery getAllCartsQuery)
    {
        ApplicationResult<Paging<CartDto>> getAllCartResult = await Sender.Send(getAllCartsQuery).ConfigureAwait(false);
        return ApplicationResultToActionResult(getAllCartResult, x => x.Value!);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetCartById(Guid id)
    {
        ApplicationResult<CartDto> getCartByIdResult = await Sender.Send(new GetCartByIdQuery { Id = id }).ConfigureAwait(false);
        return ApplicationResultToActionResult(getCartByIdResult, x => x.Value!);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCartCommand createCartCommand)
    {
        ApplicationResult<CartDto> createCartResult = await Sender.Send(createCartCommand).ConfigureAwait(false);
        return ApplicationResultToActionResult(createCartResult, x => x.Value);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCartCommand updateCartCommand)
    {
        updateCartCommand.Id = id;
        ApplicationResult<CartDto> updateCartResult = await Sender.Send(updateCartCommand).ConfigureAwait(false);
        return ApplicationResultToActionResult(updateCartResult, x => x.Value);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        ApplicationResult deleteCartResult = await Sender.Send(new DeleteCartByIdCommand { Id = id }).ConfigureAwait(false);
        return ApplicationResultToActionResult(deleteCartResult);
    }
}