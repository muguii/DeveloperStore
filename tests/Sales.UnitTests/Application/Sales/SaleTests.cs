using AutoMapper;
using Bogus;
using NSubstitute;
using Sales.Application.Sales.Commands.Create;
using Sales.Application.Sales.Commands.Update;
using Sales.Application.Sales.Queries.GetById;
using Sales.Application.Sales.Shared;
using Sales.Application.Services;
using Sales.Domain.Products;
using Sales.Domain.Sales;

namespace Sales.UnitTests.Application.Sales;

public class SaleTests
{
    private readonly Faker _faker;
    private readonly List<Sale> _sales;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly CreateSaleCommandHandler _createHandler;
    private readonly GetSaleByIdQueryHandler _getByIdHandler;
    private readonly UpdateSaleCommandHandler _updateHandler;

    public SaleTests()
    {
        _faker = new Faker();

        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();

        _createHandler = new CreateSaleCommandHandler(_unitOfWork, _mapper);
        _getByIdHandler = new GetSaleByIdQueryHandler(_unitOfWork, _mapper);
        _updateHandler = new UpdateSaleCommandHandler(_unitOfWork, _mapper);

        _sales =
        [
            Sale.Create(_faker.Person.FirstName, _faker.Company.CompanyName()).Value,
            Sale.Create(_faker.Person.FirstName, _faker.Company.CompanyName()).Value
        ];
    }

    [Fact]
    public async Task InputDataIsOk_Handle_CreateSaleSuccessfully()
    {
        // Arrange
        var product = Product.Create(_faker.Commerce.ProductName(),
                                     _faker.Finance.Amount(1, 1000),
                                     _faker.Commerce.ProductDescription(),
                                     _faker.Commerce.Categories(1).First(),
                                     "imagem",
                                     Rating.Create(5, 22).Value!).Value;
        var command = new CreateSaleCommand
        {
            Customer = _faker.Person.FirstName,
            Branch = _faker.Company.CompanyName(),
            Products = [new CreateSaleItem { Quantity = 5, ProductId = _faker.Random.Guid() }]
        };
        _unitOfWork.Product.GetByIdAsync(Arg.Is(command.Products.First().ProductId), CancellationToken.None).Returns(product);

        // Act
        var result = await _createHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);

        await _unitOfWork.Sale.Received(1).AddAsync(Arg.Any<Sale>(), CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task SaleExists_Handle_ReturnSale()
    {
        // Arrange
        var sale = _sales.First();
        _unitOfWork.Sale.GetByIdWithProductsAsync(Arg.Is(sale.Id), CancellationToken.None).Returns(sale);
        _mapper.Map<SaleDto>(Arg.Is(sale)).Returns(new SaleDto { Id = sale.Id });

        // Act
        var result = await _getByIdHandler.Handle(new GetSaleByIdQuery { Id = sale.Id }, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(sale.Id, result.Value!.Id);

        await _unitOfWork.Sale.Received(1).GetByIdWithProductsAsync(Arg.Is(sale.Id), CancellationToken.None);
    }

    [Fact]
    public async Task InputDataIsOkAndSaleExists_Handle_UpdateSaleSuccessfully()
    {
        // Arrange
        var sale = _sales.First();
        var command = new UpdateSaleCommand
        {
            Id = sale.Id,
            Customer = _faker.Person.FirstName,
            Branch = _faker.Company.CompanyName()
        };
        _unitOfWork.Sale.GetByIdWithProductsAsync(Arg.Is(sale.Id), CancellationToken.None).Returns(sale);
        _mapper.Map<SaleDto>(Arg.Is(sale)).Returns(new SaleDto { Id = sale.Id });

        // Act
        var result = await _updateHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(sale.Id, result.Value!.Id);

        await _unitOfWork.Sale.Received(1).GetByIdWithProductsAsync(Arg.Is(sale.Id), CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
