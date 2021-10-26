using System;
using AA.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AA.Web.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Order>()
				.HasIndex(e => e.OrderId)
				.IsUnique();
		}

		public virtual DbSet<Order> Orders { get; set; }

		public virtual DbSet<OrderItem> OrderItems { get; set; }
	}
}