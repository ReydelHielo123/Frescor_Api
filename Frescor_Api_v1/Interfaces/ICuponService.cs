using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;

namespace Frescor_Api_v1.Interfaces
{
	public interface ICuponService
	{
		Task<ApiResponse<PaginatedResponse<Cupone>>> ObtenerTodosAsync(
			string? codigo, string? descripcion, decimal? porcentaje, int pagina, int tamañoPagina);
		Task<ApiResponse<Cupone>> CrearAsync(Cupone cupon);
		Task<ApiResponse<Cupone>> ActualizarAsync(int id, Cupone cupon);
		Task<ApiResponse<bool>> EliminarAsync(int id);
		Task<ApiResponse<Cupone>> ObtenerPorIdAsync(int id);
	}
}