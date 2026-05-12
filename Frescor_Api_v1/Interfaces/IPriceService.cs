using Frescor_Api_v1.Models;

namespace Frescor_Api_v1.Interfaces
{
	public interface IPriceService
	{
		Task<ApiResponse<List<Precio>>> GetAllPricesAsync();
		Task<ApiResponse<Precio>> UpdatePriceAsync(int id, Precio precio);
	}
}