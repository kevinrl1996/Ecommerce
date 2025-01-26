using AutoMapper;
using Discount.Core.Entities;
using Discount.Core.Interfaces;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.API.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		private readonly IMapper _mapper;
		private readonly IDiscountRepository _discountRepository;

		public DiscountService(IMapper mapper, IDiscountRepository discountRepository)
		{
			_mapper = mapper;
			_discountRepository = discountRepository;
		}

		public async Task<CouponModel> GetDiscount(string productName)
		{
			var coupon = await _discountRepository.GetDiscount(productName);

			if (coupon == null)
			{
				throw new RpcException(new Status(StatusCode.NotFound, $"Descuento para el producto {productName} no encontrado"));
			}

			var couponModel = new CouponModel
			{
				Id = coupon.Id,
				Amount = coupon.Amount,
				Description = coupon.Description,
				ProductName = coupon.ProductName
			};

			return couponModel;
		}

		public async Task<CouponModel> CreateDiscount(CouponModel couponModel)
		{
			var coupon = new CouponModel
			{
				Amount = couponModel.Amount,
				Description = couponModel.Description,
				ProductName = couponModel.ProductName
			};

			var newCoupon = _mapper.Map<Coupon>(coupon);
			await _discountRepository.CreateDiscount(newCoupon);
			couponModel = _mapper.Map<CouponModel>(coupon);

			return couponModel;
		}

		public async Task<CouponModel> UpdateDiscount(CouponModel couponModel)
		{
			var coupon = new CouponModel
			{
				Id = couponModel.Id,
				ProductName = couponModel.ProductName,
				Description = couponModel.Description,
				Amount = couponModel.Amount
			};

			var newCoupon = _mapper.Map<Coupon>(coupon);
			await _discountRepository.UpdateDiscount(newCoupon);
			var couponModelResp = _mapper.Map<CouponModel>(newCoupon);

			return couponModelResp;
		}

		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var deleted = await _discountRepository.DeleteDiscount(request.ProductName);

			var response = new DeleteDiscountResponse
			{
				Success = deleted
			};

			return response;
		}
	}
}