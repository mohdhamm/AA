using System.Threading.Tasks;

namespace AA.Web.Interfaces
{
	public interface IBaseService<T> where T : class
	{
		Task AddAsync(T model);
	}
}