using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;

namespace Frescor_Api_v1.Interfaces
{
	public interface ITelefonoDireccionService
	{
		Task<ApiResponse<List<TelefonoDireccion>>> ObtenerDireccionesPorTelefonoAsync(string telefono);
		Task<ApiResponse<PaginatedResponse<TelefonoDireccion>>> ObtenerTodosAsync(string? telefono, string? direccion, int? zona, int pagina, int tamañoPagina);
		Task<ApiResponse<TelefonoDireccion>> CrearAsync(TelefonoDireccion cliente);
		Task<ApiResponse<TelefonoDireccion>> ActualizarAsync(int id, TelefonoDireccion cliente);
		Task<ApiResponse<bool>> EliminarAsync(int id);
	}
}