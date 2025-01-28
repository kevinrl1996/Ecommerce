using AutoMapper;
using Basket.Application.GrpcService;
using Basket.Core.DTOs;
using Basket.Core.Entities;
using Basket.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
	public class BasketController : ApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly DiscountGrpcService _discountGrpcService;
		private readonly IMapper _mapper;

		public BasketController(IMapper mapper, IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
		{
			_mapper = mapper;
			_basketRepository = basketRepository;
			_discountGrpcService = discountGrpcService;
		}

		[HttpGet]
		[Route("[action]/{userName}", Name = "GetBasketByUserName")]
		[ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCartDto>> GetBasket(string userName)
		{
			var basket = await _basketRepository.GetBasket(userName);
			var basketDto = _mapper.Map<ShoppingCartDto>(basket);

			return Ok(basketDto);
		}

		[HttpPost]
		[Route("CreateBasket")]
		[ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCartDto>> UpdateBasket([FromBody] ShoppingCartDto shoppingCartDto)
		{
			foreach (var item in shoppingCartDto.Items)
			{
				var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
				item.Price -= coupon.Amount;
			}

			var shoppingCart = await _basketRepository.UpdateBasket(new ShoppingCart
			{
				UserName = shoppingCartDto.UserName,
				Items = shoppingCartDto.Items
			});

			var newShoppingCartDto = _mapper.Map<ShoppingCart>(shoppingCart);
			await _basketRepository.UpdateBasket(newShoppingCartDto);
			shoppingCartDto = _mapper.Map<ShoppingCartDto>(shoppingCart);

			return Ok(shoppingCartDto);
		}

		[HttpDelete]
		[Route("{userName}", Name = "DeleteBasketByUserName")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<string>> DeleteBasket(string userName)
		{
			await _basketRepository.DeleteBasket(userName);
			return Ok("Carrito eliminado con exito.");
		}
	}
}
