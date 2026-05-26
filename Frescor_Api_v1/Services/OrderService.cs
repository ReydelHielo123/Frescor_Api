using Frescor_Api_v1.Data;
using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Microsoft.EntityFrameworkCore;
using Frescor_Api_v1.Models.Responses;

namespace Frescor_Api_v1.Services
{
	public class OrderService : IOrderService
	{
		private readonly AppDbContext _context;
		private readonly ILogger<OrderService> _logger;

		public OrderService(AppDbContext context, ILogger<OrderService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<ApiResponse<List<Order>>> GetAllOrdersAsync()
		{
			try
			{
				var orders = await _context.Orders.ToListAsync();
				return new ApiResponse<List<Order>>
				{
					Success = true,
					Message = "Todos los pedidos devueltos exitosamente",
					Data = orders
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al devolver todos los pedidos");
				return new ApiResponse<List<Order>>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<Pedido2>> GetOrderByIdAsync(int id)
		{
			try
			{
				var pedido = await _context.Pedido2.FirstOrDefaultAsync(x => x.Id == id);
				if (pedido == null)
					return new ApiResponse<Pedido2> { Success = false, Message = "Pedido no encontrado", Data = null };
				return new ApiResponse<Pedido2> { Success = true, Data = pedido };
			}
			catch (Exception ex)
			{
				return new ApiResponse<Pedido2> { Success = false, Message = ex.Message, Data = null };
			}
		}

		public async Task<ApiResponse<Pedido2>> InsertOrderAsync(Pedido2 order)
		{
			try
			{
				_context.Pedido2.Add(order);
				await _context.SaveChangesAsync();
				return new ApiResponse<Pedido2>
				{
					Success = true,
					Message = "Pedido insertado correctamente",
					Data = order
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al insertar pedido");
				return new ApiResponse<Pedido2>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<PaginatedResponse<Pedido2>>> FiltrarPedidosAsync(
			DateTime? desde, DateTime? hasta, string? direccion,
			int? zona, string? formaPago, string? estado,
			int pagina, int tamañoPagina, string? telefono = null)
		{
			try
			{
				var query = _context.Pedido2.AsQueryable();

				if (!desde.HasValue && !hasta.HasValue)
				{
					query = query.Where(p => p.Fecha_Carga >= DateTime.Today);
				}
				else
				{
					if (desde.HasValue)
						query = query.Where(p => p.Fecha_Carga >= desde.Value.Date);

					if (hasta.HasValue)
						query = query.Where(p => p.Fecha_Carga < hasta.Value.Date.AddDays(1));
				}

				if (!string.IsNullOrEmpty(direccion))
					query = query.Where(p => p.Direccion!.Contains(direccion));

				if (zona.HasValue)
					query = query.Where(p => p.Zona == zona.Value);

				if (!string.IsNullOrEmpty(formaPago))
					query = query.Where(p => p.FormaPago == formaPago);

				if (!string.IsNullOrEmpty(estado))
					query = query.Where(p => p.Estado == estado);

				if (!string.IsNullOrEmpty(telefono))
					query = query.Where(p => p.Telefono == telefono);

				var totalRegistros = await query.CountAsync();

				var pedidos = await query
					.OrderByDescending(p => p.Fecha_Carga)
					.Skip((pagina - 1) * tamañoPagina)
					.Take(tamañoPagina)
					.ToListAsync();

				return new ApiResponse<PaginatedResponse<Pedido2>>
				{
					Success = true,
					Message = "Pedidos filtrados correctamente",
					Data = new PaginatedResponse<Pedido2>
					{
						Items = pedidos,
						TotalRegistros = totalRegistros,
						Pagina = pagina,
						TamañoPagina = tamañoPagina
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al filtrar pedidos");
				return new ApiResponse<PaginatedResponse<Pedido2>>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<object>> GetMetricasAsync()
		{
			try
			{
				var hoy = DateTime.Today;

				var pedidosHoy = await _context.Pedido2
					.Where(p => p.Fecha_Carga >= hoy)
					.ToListAsync();

				var totalHoy = pedidosHoy.Sum(p =>
					double.TryParse(
						p.Precio_Total,
						System.Globalization.NumberStyles.Any,
						System.Globalization.CultureInfo.InvariantCulture,
						out var val) ? val : 0);
				var pedidosPagados = pedidosHoy.Count(p => p.Estado == "Pagado");
				var pedidosPendientes = pedidosHoy.Count(p => p.Estado == "PendientePago");
				var pedidosEfectivo = pedidosHoy.Count(p => p.FormaPago == "efectivo");
				var pedidosMP = pedidosHoy.Count(p => p.FormaPago == "mercadopago");

				return new ApiResponse<object>
				{
					Success = true,
					Message = "Métricas obtenidas correctamente",
					Data = new
					{
						pedidosHoy = pedidosHoy.Count,
						totalHoy,
						pedidosPagados,
						pedidosPendientes,
						pedidosEfectivo,
						pedidosMP
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener métricas");
				return new ApiResponse<object>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}
	}
}