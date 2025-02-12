using AutoMapper;
using Bogus;
using NSubstitute;
using Sales.Application.Carts.Commands.Create;
using Sales.Application.Carts.Commands.Update;
using Sales.Application.Carts.Queries.GetById;
using Sales.Application.Carts.Shared;
using Sales.Application.Services;
using Sales.Domain.Carts;
using Sales.Domain.Products;

namespace Sales.UnitTests.Application.Carts;

public class CartTests
{
    private readonly Faker _faker;
    private readonly List<Cart> _carts;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly CreateCartCommandHandler _createHandler;
    private readonly GetCartByIdQueryHandler _getByIdHandler;
    private readonly UpdateCartCommandHandler _updateHandler;

    public CartTests()
    {
        _faker = new Faker();

        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();

        _createHandler = new CreateCartCommandHandler(_unitOfWork, _mapper);
        _getByIdHandler = new GetCartByIdQueryHandler(_unitOfWork, _mapper);
        _updateHandler = new UpdateCartCommandHandler(_unitOfWork, _mapper);

        _carts =
        [
            Cart.Create(_faker.Random.Int(1), _faker.Date.Recent()).Value,
            Cart.Create(_faker.Random.Int(1), _faker.Date.Recent()).Value
        ];
    }

    [Fact]
    public async Task InputDataIsOk_Handle_CreateCartSuccessfully()
    {
        // Arrange
        var product = Product.Create(_faker.Commerce.ProductName(),
                                     _faker.Finance.Amount(1, 1000),
                                     _faker.Commerce.ProductDescription(),
                                     _faker.Commerce.Categories(1).First(),
                                     "imagem",
                                     Rating.Create(5, 22).Value!).Value;
        var command = new CreateCartCommand
        {
            UserId = _faker.Random.Int(1),
            Date = _faker.Date.Recent(),
            Products = [ new CreateCartItem { Quantity = 5, ProductId = _faker.Random.Guid() }]
        };
        _unitOfWork.Product.GetByIdAsync(Arg.Is(command.Products.First().ProductId), CancellationToken.None).Returns(product);

        // Act
        var result = await _createHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);

        await _unitOfWork.Cart.Received(1).AddAsync(Arg.Any<Cart>(), CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task CartExists_Handle_ReturnCart()
    {
        // Arrange
        var cart = _carts.First();
        _unitOfWork.Cart.GetByIdWithProductsAsync(Arg.Is(cart.Id), CancellationToken.None).Returns(cart);
        _mapper.Map<CartDto>(Arg.Is(cart)).Returns(new CartDto { Id = cart.Id });

        // Act
        var result = await _getByIdHandler.Handle(new GetCartByIdQuery { Id = cart.Id }, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(cart.Id, result.Value!.Id);

        await _unitOfWork.Cart.Received(1).GetByIdWithProductsAsync(Arg.Is(cart.Id), CancellationToken.None);
    }

    [Fact]
    public async Task InputDataIsOkAndCartExists_Handle_UpdateCartSuccessfully()
    {
        // Arrange
        var product = Product.Create(_faker.Commerce.ProductName(),
                                     _faker.Finance.Amount(1, 1000),
                                     _faker.Commerce.ProductDescription(),
                                     _faker.Commerce.Categories(1).First(),
                                     "imagem",
                                     Rating.Create(5, 22).Value!).Value;
        var cart = _carts.First();
        var command = new UpdateCartCommand
        {
            Id = cart.Id,
            UserId = _faker.Random.Int(1),
            Date = _faker.Date.Recent(),
            Products = [new UpdateCartItem { Quantity = 5, ProductId = _faker.Random.Guid() }]
        };
        _unitOfWork.Cart.GetByIdWithProductsAsync(Arg.Is(cart.Id), CancellationToken.None).Returns(cart);
        _unitOfWork.Product.GetByIdAsync(Arg.Is(command.Products.First().ProductId), CancellationToken.None).Returns(product);
        _mapper.Map<CartDto>(Arg.Is(cart)).Returns(new CartDto { Id = cart.Id });

        // Act
        var result = await _updateHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(cart.Id, result.Value!.Id);

        await _unitOfWork.Cart.Received(1).GetByIdWithProductsAsync(Arg.Is(cart.Id), CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
