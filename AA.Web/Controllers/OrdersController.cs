using System;
using System.Threading.Tasks;
using AA.Web.Interfaces;
using AA.Shared.Dtos.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AA.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrdersController(
			IOrderService orderService)
			: base()
		{
			_orderService = orderService;
		}

		[HttpPost]
		public async Task<ActionResult> CreateOrder(OrderForNew order)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				await _orderService.CreateOrderAsync(order);
			}
			catch(InvalidOperationException e)
			{
				return BadRequest(e.Message);
			}
			catch(DbUpdateException)
			{
				return BadRequest("Order id already exists");
			}

			return Ok();
		}

		[HttpGet]
		public async Task<ActionResult<OrderForView>> GetOrderDetails(string orderId)
		{
			var orderForView = await _orderService.GetOrderDetailsAsync(orderId);

			if (orderForView == null)
			{
				return NotFound("No order with this Id");
			}

			return Ok(orderForView);
		}
	}
}