using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;
using Ordering.Core.DTOs;
using Ordering.Core.Entities;
using Ordering.Core.Interfaces;

namespace Ordering.API.EventBus.Consumer
{
	public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<BasketOrderingConsumer> _logger;
		private readonly IOrderRepository _orderRepository;

		public BasketOrderingConsumer(IMapper mapper, ILogger<BasketOrderingConsumer> logger, IOrderRepository orderRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_orderRepository = orderRepository;
		}

		public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
		{
			using var scope = _logger.BeginScope("Consumo de evento de pago de carrito {correlationId}", context.Message.CorrelationId);
			var cmd = _mapper.Map<Order>(context.Message);
			await _orderRepository.AddAsync(cmd);
			_logger.LogInformation("¡¡¡Evento de pago de carrito completado!!!");
		}
	}
}
