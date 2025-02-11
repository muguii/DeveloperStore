using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Products.Commands.Create;
using Sales.Application.Products.Commands.DeleteById;
using Sales.Application.Products.Commands.Update;
using Sales.Application.Products.Queries.GetAll;
using Sales.Application.Products.Queries.GetAllCategories;
using Sales.Application.Products.Queries.GetByCategory;
using Sales.Application.Products.Queries.GetById;
using Sales.Application.Products.Shared;
using Sales.Application.Shared;

namespace Sales.API.Products;

[Route("api/products")]
public sealed class ProductController : Shared.ControllerBase
{
    public ProductController(ISender sender) : base(sender)
    {

    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery getAllProductsQuery)
    {
        ApplicationResult<Paging<ProductDto>> getAllProductResult = await Sender.Send(getAllProductsQuery).ConfigureAwait(false);
        return base.ApplicationResultToActionResult(getAllProductResult, x => x.Value!);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        ApplicationResult<ProductDto> getProductByIdResult = await Sender.Send(new GetProductByIdQuery { Id = id }).ConfigureAwait(false);
        return base.ApplicationResultToActionResult(getProductByIdResult, x => x.Value!);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category, [FromQuery] PagingFilter pagingFilter, [FromQuery] string? OrderBy)
    {
        var getProductByCategory = new GetProductByCategoryQuery { Category = category, Page = pagingFilter.Page, Size = pagingFilter.Size, OrderBy = OrderBy };
        ApplicationResult<Paging<ProductDto>> getProductByCategoryResult = await Sender.Send(getProductByCategory).ConfigureAwait(false);
        return base.ApplicationResultToActionResult(getProductByCategoryResult, x => x.Value!);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        ApplicationResult<List<string>> getAllCategoriesResult = await Sender.Send(new GetAllCategoriesQuery()).ConfigureAwait(false);
        return base.ApplicationResultToActionResult(getAllCategoriesResult, x => x.Value!);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand createProductCommand)
    {
        ApplicationResult<ProductDto> createProductResult = await Sender.Send(createProductCommand).ConfigureAwait(false);
        return base.ApplicationResultToActionResult(createProductResult, x => x.Value);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand updateProductCommand)
    {
        updateProductCommand.Id = id;
        ApplicationResult<ProductDto> updateProductResult = await Sender.Send(updateProductCommand).ConfigureAwait(false);
        return base.ApplicationResultToActionResult(updateProductResult, x => x.Value);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        ApplicationResult deleteProductResult = await Sender.Send(new DeleteProductByIdCommand { Id = id }).ConfigureAwait(false);
        return base.ApplicationResultToActionResult(deleteProductResult);
    }
}