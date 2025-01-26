using AutoMapper;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Infrastructure.Mappers
{
	public class DiscountMappingProfile : Profile
	{
		public DiscountMappingProfile()
		{
			CreateMap<Coupon, CouponModel>().ReverseMap();
		}
	}
}
