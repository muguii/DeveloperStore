using AutoMapper;
using Sales.Domain.Carts;

namespace Sales.Application.Carts.Shared;

internal sealed class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>();
        CreateMap<CartItem, CartItemDto>();
    }
}