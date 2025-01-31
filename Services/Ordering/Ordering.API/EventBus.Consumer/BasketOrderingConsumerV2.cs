using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.Core.Entities;
using Ordering.Core.Interfaces;

namespace Ordering.API.EventBus.Consumer
{
	public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<BasketOrderingConsumerV2> _logger;
		private readonly IOrderRepository _orderRepository;

		public BasketOrderingConsumerV2(IMapper mapper, ILogger<BasketOrderingConsumerV2> logger, IOrderRepository orderRepository)
		{
			_mapper = mapper;
			_logger = logger;
			_orderRepository = orderRepository;
		}

		public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
		{
			using var scope = _logger.BeginScope("Consumo de evento de pago de carrito {correlationId}", context.Message.CorrelationId);
			var cmd = _mapper.Map<Order>(context.Message);
			await _orderRepository.AddAsync(cmd);
			_logger.LogInformation("¡¡¡Evento de pago de carrito completado!!!");
		}
	}
}
