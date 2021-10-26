using System;
using System.Linq;
using System.Threading.Tasks;
using AA.Web.Models;
using AA.Web.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AA.Web.Services
{
	public class BaseService<T> : IBaseService<T> where T : BaseEntity
	{
		private readonly DbContext _context;

		public BaseService(DbContext context)
		{
			_context = context;
		}

		public virtual async Task AddAsync(T model)
		{
			model.ModifiedAt = DateTime.Now;

			_context.Add(model);
			await _context.SaveChangesAsync();
		}
	}
}