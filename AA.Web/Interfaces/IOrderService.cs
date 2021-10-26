using System.Threading.Tasks;
using AA.Web.Models;
using AA.Shared.Dtos.Order;

namespace AA.Web.Interfaces
{
	public interface IOrderService : IBaseService<Order>
	{
		Task<Order> CreateOrderAsync(OrderForNew @new);

		Task<OrderForView> GetOrderDetailsAsync(string orderId);

		double CalculatePackageWidth(Order order);
	}
}