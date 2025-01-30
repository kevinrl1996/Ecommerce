using AutoMapper;
using Basket.Core.DTOs;
using Basket.Core.Entities;
using Basket.Core.Interfaces;
using Basket.Infrastructure.GrpcService;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
	public class BasketController : ApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly DiscountGrpcService _discountGrpcService;
		private readonly IMapper _mapper;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly ILogger<BasketController> _logger;

		public BasketController(IMapper mapper, IPublishEndpoint publishEndpoint, IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, ILogger<BasketController> logger)
		{
			_mapper = mapper;
			_publishEndpoint = publishEndpoint;
			_basketRepository = basketRepository;
			_discountGrpcService = discountGrpcService;
			_logger = logger;
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

		[Route("[action]")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Accepted)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
		{
			// Obtener el carrito existente
			var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
			var basketDto = _mapper.Map<ShoppingCartDto>(basket);

			if (basketDto == null)
			{
				return BadRequest();
			}

			var eventMsg = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
			eventMsg.TotalPrice = basketDto.TotalPrice;
			await _publishEndpoint.Publish(eventMsg);
			_logger.LogInformation($"Carrito publicado por {basketDto.UserName}");

			// Eliminar el carrito
			var deleteCmd = _basketRepository.DeleteBasket(basketCheckout.UserName);
			return Accepted();
		}
	}
}
