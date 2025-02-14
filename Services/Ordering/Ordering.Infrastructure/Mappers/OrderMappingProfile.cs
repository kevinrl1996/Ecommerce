﻿using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Core.DTOs;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Mappers
{
	public class OrderMappingProfile : Profile
	{
		public OrderMappingProfile()
		{
			CreateMap<Order, OrderDto>().ReverseMap();
			CreateMap<Order, CheckOutOrderDto>().ReverseMap();
			CreateMap<Order, UpdateOrderDto>().ReverseMap();

			CreateMap<Order, BasketCheckoutEvent>().ReverseMap();
			CreateMap<Order, BasketCheckoutEventV2>().ReverseMap();

			CreateMap<CheckOutOrderDto, BasketCheckoutEvent>().ReverseMap();
		}
	}
}
