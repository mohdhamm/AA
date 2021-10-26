using System;
using System.Linq;
using System.Threading.Tasks;
using AA.Shared.Constants;
using AA.Shared.Dtos.Order;
using AA.Shared.Enums;
using AA.Web.Interfaces;
using AA.Web.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AA.Web.Services
{
	public class OrderService : BaseService<Order>, IOrderService
	{
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public OrderService(AppDbContext dbContext,
			IMapper mapper
			) : base(dbContext)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<Order> CreateOrderAsync(OrderForNew @new)
		{
			Validate(@new);

			var newOrder = _mapper.Map<Order>(@new);
			await AddAsync(newOrder);

			return newOrder;
		}

		public async Task<OrderForView> GetOrderDetailsAsync(string orderId)
		{
			var order = await _dbContext.Orders
				.Include(o => o.OrderItems)
				.FirstOrDefaultAsync(o => o.OrderId == orderId);

			if (order == null)
			{
				return null;
			}

			var orderForView = _mapper.Map<OrderForView>(order);
			orderForView.PackageWidth = CalculatePackageWidth(order);

			return orderForView;
		}

		public double CalculatePackageWidth(Order order)
		{
			var packageWidth = 0.0;
			var itemWidths = OrderConstants.ItemWidths;

			foreach (var item in order.OrderItems)
			{
				packageWidth += item.ProductType != ProductType.Mug ?
					itemWidths[item.ProductType] * item.Quantity :
					(itemWidths[item.ProductType] * Math.Ceiling(item.Quantity / 4.0));
			}

			return packageWidth;
		}

		private void Validate(OrderForNew @new)
		{
			if (@new.OrderItems == null || !@new.OrderItems.Any())
			{
				throw new InvalidOperationException("Order should have at least 1 item");
			}

			if (@new.OrderItems.Any(item => item.Quantity <= 0))
			{
				throw new InvalidOperationException("Quantity must be greater than 0");
			}
		}
	}
}