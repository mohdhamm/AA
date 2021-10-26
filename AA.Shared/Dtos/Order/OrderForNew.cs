using System.Collections.Generic;
using AA.Shared.Dtos.OrderItem;

namespace AA.Shared.Dtos.Order
{
	public class OrderForNew
	{
		public string OrderId { get; set; }

		public IEnumerable<OrderItemForNew> OrderItems { get; set; }
	}
}