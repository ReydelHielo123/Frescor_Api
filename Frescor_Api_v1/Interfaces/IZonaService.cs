using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;

namespace Frescor_Api_v1.Interfaces
{
	public interface IZonaService
	{
		Task<ApiResponse<List<Zona>>> ObtenerTodasAsync();
		Task<ApiResponse<Zona>> ActualizarAsync(int id, Zona zona);
		Task<ApiResponse<Zona>> CrearAsync(Zona zona);
		Task<ApiResponse<bool>> EliminarAsync(int id);
	}
}