using System.Collections.Generic;
using AA.Shared.Dtos.OrderItem;

namespace AA.Shared.Dtos.Order
{
	public class OrderForView
	{
		public string OrderId { get; set; }

		public double PackageWidth { get; set; }

		public List<OrderItemForView> OrderItems { get; set; }
	}
}