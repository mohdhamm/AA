using AA.Shared.Enums;

namespace AA.Web.Models
{
	public class OrderItem : BaseEntity
	{
		public int OrderId { get; set; }
		public virtual Order Order { get; set; }

		public int Quantity { get; set; }

		public ProductType ProductType { get; set; }
	}
}