using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AA.Shared.Dtos.Order;
using AA.Shared.Dtos.OrderItem;
using AA.Web.Models;
using AA.Web.Profiles;
using AA.Web.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AA.Test
{
	public class OrderServiceTest
	{
		private readonly Mock<AppDbContext> _mockDbContext;

		private readonly IMapper _mapper;

		private readonly OrderService _orderService;

		public OrderServiceTest()
		{
			var options = new DbContextOptions<AppDbContext>();
			_mockDbContext = new Mock<AppDbContext>(options);

			var mockSet = new Mock<DbSet<Order>>();
			_mockDbContext.Setup(db => db.Orders).Returns(mockSet.Object);

			var config = new MapperConfiguration(cfg => cfg.AddProfile<OrderProfile>());
			_mapper = new Mapper(config);

			_orderService = new OrderService(_mockDbContext.Object, _mapper);
		}

		[Fact]
		public async Task CreateOrderAsync_Order_CreatesSuccessfully()
		{
			var modelForNew = new OrderForNew()
			{
				OrderId = "a",
				OrderItems = new List<OrderItemForNew>()
				{
					new OrderItemForNew() { Quantity = 1 }
				}
			};

			var model = await _orderService.CreateOrderAsync(modelForNew);

			_mockDbContext.Verify(m => m.Add(model));
			_mockDbContext.Verify(m => m.SaveChangesAsync(default));
		}

		[Fact]
		public async Task AddAsync_Order_CreatesSuccessfully()
		{
			var model = new Order();

			await _orderService.AddAsync(model);

			_mockDbContext.Verify(m => m.Add(model));
			_mockDbContext.Verify(m => m.SaveChangesAsync(default));
		}

		[Fact]
		public async Task CreateOrderAsync_QuantityZero_Throws()
		{
			var modelForNew = new OrderForNew()
			{
				OrderId = "a",
				OrderItems = new List<OrderItemForNew>()
				{
					new OrderItemForNew() { Quantity = 0 }
				}
			};

			await Assert.ThrowsAsync<InvalidOperationException>(async () => await _orderService.CreateOrderAsync(modelForNew));
		}

		[Fact]
		public async Task CreateOrderAsync_NullItems_Throws()
		{
			var modelemptyItems = new OrderForNew()
			{
				OrderId = "a",
				OrderItems = null
			};

			await Assert.ThrowsAsync<InvalidOperationException>(async () => await _orderService.CreateOrderAsync(modelemptyItems));
		}

		[Fact]
		public async Task CreateOrderAsync_EmptyItems_Throws()
		{
			var modelemptyItems = new OrderForNew()
			{
				OrderId = "a",
				OrderItems = new List<OrderItemForNew>()
			};

			await Assert.ThrowsAsync<InvalidOperationException>(async () => await _orderService.CreateOrderAsync(modelemptyItems));
		}

		[Theory]
		[ClassData(typeof(OrderClassData))]
		public void CalculatePackageWidth(Order order, double expected)
		{
			var actual = _orderService.CalculatePackageWidth(order);

			Assert.Equal(expected, actual);
		}
	}

	public class OrderClassData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] {
				new Order() {
					OrderItems = new List<OrderItem>() {
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Calendar,
							Quantity = 1
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.PhotoBook,
							Quantity = 2
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Mug,
							Quantity = 3
						},
					}
				},
				142
			};
			yield return new object[] {
				new Order() {
					OrderItems = new List<OrderItem>() {
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Canvas,
							Quantity = 3
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.PhotoBook,
							Quantity = 2
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Mug,
							Quantity = 5
						},
					}
				},
				274
			};
			yield return new object[] {
				new Order() {
					OrderItems = new List<OrderItem>() {
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Calendar,
							Quantity = 3
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.PhotoBook,
							Quantity = 5
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Cards,
							Quantity = 9
						},
					}
				},
				167.3
			};
			yield return new object[] {
				new Order() {
					OrderItems = new List<OrderItem>() {
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Canvas,
							Quantity = 3
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.PhotoBook,
							Quantity = 1
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Cards,
							Quantity = 3
						},
						new OrderItem()
						{
							ProductType = Shared.Enums.ProductType.Mug,
							Quantity = 9
						},
					}
				}, 
				363.1
			};
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}