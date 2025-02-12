using AutoMapper;
using Sales.Domain.Sales;

namespace Sales.Application.Sales.Shared;

internal sealed class SaleProfile : Profile
{
	public SaleProfile()
	{
        CreateMap<Sale, SaleDto>();
        CreateMap<SaleItem, SaleItemDto>();
    }
}