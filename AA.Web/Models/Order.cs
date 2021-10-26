using System.Collections.Generic;

namespace AA.Web.Models
{
	public class Order : BaseEntity
	{
		public string OrderId { get; set; }

		public IEnumerable<OrderItem> OrderItems { get; set; }
	}
}