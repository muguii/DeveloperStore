using AutoMapper;
using Sales.Domain.Products;

namespace Sales.Application.Products.Shared;

internal sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Rating, RatingDto>();
        CreateMap<Product, ProductDto>();
    }
}