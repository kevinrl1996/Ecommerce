using Asp.Versioning;
using AutoMapper;
using Basket.Core.DTOs;
using Basket.Core.Entities;
using Basket.Core.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
	[ApiVersion("2")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly ILogger<BasketController> _logger;

		public BasketController(IMapper mapper, IPublishEndpoint publishEndpoint, IBasketRepository basketRepository, ILogger<BasketController> logger)
		{
			_mapper = mapper;
			_publishEndpoint = publishEndpoint;
			_basketRepository = basketRepository;
			_logger = logger;
		}

		[Route("[action]")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Accepted)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
		{
			// Obtener el carrito existente
			var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
			var basketDto = _mapper.Map<ShoppingCartDto>(basket);

			if (basketDto == null)
			{
				return BadRequest();
			}

			var eventMsg = _mapper.Map<BasketCheckoutEventV2>(basketCheckout);
			eventMsg.TotalPrice = basketDto.TotalPrice;
			await _publishEndpoint.Publish(eventMsg);
			_logger.LogInformation($"Carrito publicado por {basketDto.UserName} con el endpoint V2");

			// Eliminar el carrito
			var deleteCmd = _basketRepository.DeleteBasket(basketCheckout.UserName);
			return Accepted();
		}
	}
}
