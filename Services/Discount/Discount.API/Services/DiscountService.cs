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
		private readonly ILogger<DiscountService> _logger;

		public DiscountService(IMapper mapper, IDiscountRepository discountRepository, ILogger<DiscountService> logger)
		{
			_mapper = mapper;
			_discountRepository = discountRepository;
			_logger = logger;
		}

		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon = await _discountRepository.GetDiscount(request.ProductName);

			if (coupon == null)
			{
				throw new RpcException(new Status(StatusCode.NotFound, $"Descuento para el producto {request.ProductName} no encontrado"));
			}

			var couponModel = new CouponModel
			{
				Id = coupon.Id,
				Amount = coupon.Amount,
				Description = coupon.Description,
				ProductName = coupon.ProductName
			};

			_logger.LogInformation($"Cupón para {request.ProductName} obtenido");

			return couponModel;
		}

		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var coupon = new CouponModel
			{
				Amount = request.Coupon.Amount,
				Description = request.Coupon.Description,
				ProductName = request.Coupon.ProductName
			};

			var newCoupon = _mapper.Map<Coupon>(coupon);
			await _discountRepository.CreateDiscount(newCoupon);
			request.Coupon = _mapper.Map<CouponModel>(coupon);

			return request.Coupon;
		}

		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var coupon = new CouponModel
			{
				Id = request.Coupon.Id,
				ProductName = request.Coupon.ProductName,
				Description = request.Coupon.Description,
				Amount = request.Coupon.Amount
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