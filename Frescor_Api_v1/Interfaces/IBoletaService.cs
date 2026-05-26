using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;

namespace Frescor_Api_v1.Interfaces
{
    public interface IBoletaService
    {
        Task<ApiResponse<List<Boleta>>> GetBoletasByTelefonoAsync(string telefono);
        Task<ApiResponse<Boleta>> CrearBoletaAsync(Boleta boleta);
        Task<ApiResponse<Boleta>> MarcarPagadaAsync(int id);
        Task<ApiResponse<bool>> EliminarBoletaAsync(int id);
        Task<ApiResponse<List<object>>> GetDeudoresAsync();
    }
}
