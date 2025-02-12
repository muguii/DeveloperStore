using AutoMapper;
using Bogus;
using NSubstitute;
using Sales.Application.Products.Commands.Create;
using Sales.Application.Products.Commands.Update;
using Sales.Application.Products.Queries.GetById;
using Sales.Application.Products.Shared;
using Sales.Application.Services;
using Sales.Domain.Products;

namespace Sales.UnitTests.Application.Products;

public class ProductTests
{
    private readonly Faker _faker;
    private readonly List<Product> _products;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly CreateProductCommandHandler _createHandler;
    private readonly GetProductByIdQueryHandler _getByIdHandler;
    private readonly UpdateProductCommandHandler _updateHandler;

    public ProductTests()
    {
        _faker = new Faker();

        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();

        _createHandler = new CreateProductCommandHandler(_unitOfWork, _mapper);
        _getByIdHandler = new GetProductByIdQueryHandler(_unitOfWork, _mapper);
        _updateHandler = new UpdateProductCommandHandler(_unitOfWork, _mapper);

        _products =
        [
            Product.Create(_faker.Commerce.ProductName(),
                           _faker.Finance.Amount(1, 1000),
                           _faker.Commerce.ProductDescription(),
                           _faker.Commerce.Categories(1).First(),
                           "imagem",
                           Rating.Create(5, 22).Value!).Value,

            Product.Create(_faker.Commerce.ProductName(),
                           _faker.Finance.Amount(1, 1000),
                           _faker.Commerce.ProductDescription(),
                           _faker.Commerce.Categories(1).First(),
                           "imagem",
                           Rating.Create(5, 22).Value!).Value!,
        ];
    }

    [Fact]
    public async Task InputDataIsOk_Handle_CreateProductSuccessfully()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Title = _faker.Commerce.ProductName(),
            Price = _faker.Finance.Amount(1, 1000),
            Description = _faker.Commerce.ProductDescription(),
            Category = _faker.Commerce.Categories(1).First(),
            Image = "imagem",
            Rating = new CreteProductRating
            {
                Rate = 3,
                Count = 1
            }
        };

        // Act
        var result = await _createHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);

        await _unitOfWork.Product.Received(1).AddAsync(Arg.Any<Product>(), CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ProductExists_Handle_ReturnProduct()
    {
        // Arrange
        var product = _products.First();
        _unitOfWork.Product.GetByIdAsync(Arg.Is(product.Id), CancellationToken.None).Returns(product);
        _mapper.Map<ProductDto>(Arg.Is(product)).Returns(new ProductDto { Id = product.Id });

        // Act
        var result = await _getByIdHandler.Handle(new GetProductByIdQuery { Id = product.Id }, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(product.Id, result.Value!.Id);

        await _unitOfWork.Product.Received(1).GetByIdAsync(Arg.Is(product.Id), CancellationToken.None);
    }

    [Fact]
    public async Task InputDataIsOkAndProductExists_Handle_UpdateProductSuccessfully()
    {
        // Arrange
        var product = _products.First();
        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Title = _faker.Commerce.ProductName(),
            Price = _faker.Finance.Amount(1, 1000),
            Description = _faker.Commerce.ProductDescription(),
            Category = _faker.Commerce.Categories(1).First(),
            Image = "imagem",
            Rating = new UpdateProductRating
            {
                Rate = 3,
                Count = 1
            }
        };
        _unitOfWork.Product.GetByIdAsync(Arg.Is(product.Id), CancellationToken.None).Returns(product);
        _mapper.Map<ProductDto>(Arg.Is(product)).Returns(new ProductDto { Id = product.Id, Title = command.Title });

        // Act
        var result = await _updateHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(product.Id, result.Value!.Id);
        Assert.Equal(product.Title, result.Value!.Title);

        await _unitOfWork.Product.Received(1).GetByIdAsync(Arg.Is(product.Id), CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}