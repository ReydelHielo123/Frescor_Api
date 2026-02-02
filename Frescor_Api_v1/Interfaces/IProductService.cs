using Frescor_Api_v1.Models;

namespace Frescor_Api_v1.Interfaces
{
	public interface IProductService
	{
		Task<ApiResponse<List<Order>>> GetAllProductsAsync();
	}
}
