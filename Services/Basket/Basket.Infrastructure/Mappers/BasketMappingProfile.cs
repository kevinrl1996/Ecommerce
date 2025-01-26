using AutoMapper;
using Basket.Core.DTOs;
using Basket.Core.Entities;

namespace Basket.Infrastructure.Mappers
{
	public class BasketMappingProfile : Profile
	{
		public BasketMappingProfile()
		{
			CreateMap<ShoppingCart, ShoppingCartDto>().ReverseMap();
			CreateMap<ShoppingCartItem, ShoppingCartItemDto>().ReverseMap();
		}
	}
}
