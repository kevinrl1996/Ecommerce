using AutoMapper;
using Catalog.Core.DTOS;
using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Infrastructure.Mappers
{
	public class ProductMappingProfile : Profile
	{
		public ProductMappingProfile()
		{
			CreateMap<ProductBrand, BrandDto>().ReverseMap();
			CreateMap<Product, ProductDto>().ReverseMap();
			CreateMap<ProductType, TypesDto>().ReverseMap();
			CreateMap<Pagination<Product>, Pagination<ProductDto>>().ReverseMap();
		}
	}
}
