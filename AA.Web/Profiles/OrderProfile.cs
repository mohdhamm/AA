using System;
using AA.Shared.Dtos.Order;
using AA.Shared.Dtos.OrderItem;
using AA.Shared.Enums;
using AA.Web.Models;
using AutoMapper;

namespace AA.Web.Profiles
{
	public class OrderProfile : Profile
	{
		public OrderProfile()
		{
			// Order
			CreateMap<Order, OrderForView>().IncludeAllDerived().ReverseMap().IncludeAllDerived();
			CreateMap<OrderForNew, Order>().ReverseMap();

			// Order Item
			CreateMap<OrderItemForView, OrderItem>().ReverseMap();
			CreateMap<OrderItemForNew, OrderItem>()
				.ForMember(o => o.ProductType, ex => ex.MapFrom(o => Enum.Parse(typeof(ProductType), o.ProductType)))
				.ReverseMap();
		}
	}
}