using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;

namespace Frescor_Api_v1.Interfaces
{
	public interface IOrderService
	{
		Task<ApiResponse<List<Order>>> GetAllOrdersAsync();
		Task<ApiResponse<Pedido2>> GetOrderByIdAsync(int id);
		Task<ApiResponse<Pedido2>> InsertOrderAsync(Pedido2 order);
		Task<ApiResponse<PaginatedResponse<Pedido2>>> FiltrarPedidosAsync(
			DateTime? desde, DateTime? hasta, string? direccion,
			int? zona, string? formaPago, string? estado,
			int pagina, int tamañoPagina);
		Task<ApiResponse<object>> GetMetricasAsync();
	}
}