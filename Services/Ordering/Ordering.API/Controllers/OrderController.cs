using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ordering.Core.DTOs;
using Ordering.Core.Entities;
using Ordering.Core.Interfaces;
using Ordering.Infrastructure.Exceptions;
using System.Net;

namespace Ordering.API.Controllers
{
	public class OrderController : ApiController
	{
		private readonly IMapper _mapper;
		private readonly ILogger<OrderController> _logger;
		private readonly IOrderRepository _orderRepository;

		public OrderController(IMapper mapper, ILogger<OrderController> logger, IOrderRepository orderRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_orderRepository = orderRepository;
		}

		[HttpGet("{userName}", Name = "GetOrdersByUserName")]
		[ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName(string userName)
		{
			var orderList = await _orderRepository.GetOrdersByUserName(userName);
			var orderListDto = _mapper.Map<List<OrderDto>>(orderList);
			return Ok(orderListDto);
		}

		//Prueba
		[HttpPost(Name = "CheckoutOrder")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckOutOrderDto checkOutOrderDto)
		{
			var orderEntity = _mapper.Map<Order>(checkOutOrderDto);
			var generatedOrder = await _orderRepository.AddAsync(orderEntity);
			_logger.LogInformation($"La Orden con el Id {generatedOrder.Id} se creó correctamente.");
			return Ok(generatedOrder.Id);
		}

		[HttpPut(Name = "UpdateOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<int>> UpdateOrder([FromBody] UpdateOrderDto updateOrderDto)
		{
			var orderToUpdate = await _orderRepository.GetByIdAsync(updateOrderDto.Id);

			if (orderToUpdate == null)
			{
				throw new OrderNotFoundException(nameof(Order), updateOrderDto.Id);
			}

			_mapper.Map(updateOrderDto, orderToUpdate, typeof(UpdateOrderDto), typeof(Order));
			await _orderRepository.UpdateAsync(orderToUpdate);
			_logger.LogInformation($"La Orden {orderToUpdate.Id} se actualizó correctamente.");
			return NoContent();
		}

		[HttpDelete("{id}", Name = "DeleteOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteOrder(int id)
		{
			var orderToDelete = await _orderRepository.GetByIdAsync(id);

			if (orderToDelete == null)
			{
				throw new OrderNotFoundException(nameof(Order), id);
			}

			await _orderRepository.DeleteAsync(orderToDelete);
			_logger.LogInformation($"La Orden {id} se eliminó correctamente.");
			return NoContent();
		}
	}
}
