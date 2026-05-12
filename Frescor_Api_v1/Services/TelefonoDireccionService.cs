using Frescor_Api_v1.Data;
using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Frescor_Api_v1.Services
{
	public class TelefonoDireccionService : ITelefonoDireccionService
	{
		private readonly AppDbContext _context;
		private readonly ILogger<TelefonoDireccionService> _logger;

		public TelefonoDireccionService(AppDbContext context, ILogger<TelefonoDireccionService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<ApiResponse<List<TelefonoDireccion>>> ObtenerDireccionesPorTelefonoAsync(string telefono)
		{
			try
			{
				var direcciones = await _context.Telefonos
					.Where(x => x.Telefono == telefono)
					.ToListAsync();

				return new ApiResponse<List<TelefonoDireccion>>
				{
					Success = true,
					Data = direcciones,
					Message = "Direcciones obtenidas correctamente"
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al devolver las direcciones");
				return new ApiResponse<List<TelefonoDireccion>>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<PaginatedResponse<TelefonoDireccion>>> ObtenerTodosAsync(
			string? telefono, string? direccion, int? zona, int pagina, int tamañoPagina)
		{
			try
			{
				var query = _context.Telefonos.AsQueryable();

				if (!string.IsNullOrEmpty(telefono))
					query = query.Where(x => x.Telefono.Contains(telefono));

				if (!string.IsNullOrEmpty(direccion))
					query = query.Where(x => x.Direccion.Contains(direccion));

				if (zona.HasValue)
					query = query.Where(x => x.Zona == zona.Value);

				var total = await query.CountAsync();

				var items = await query
					.OrderBy(x => x.Telefono)
					.Skip((pagina - 1) * tamañoPagina)
					.Take(tamañoPagina)
					.ToListAsync();

				return new ApiResponse<PaginatedResponse<TelefonoDireccion>>
				{
					Success = true,
					Message = "Clientes obtenidos correctamente",
					Data = new PaginatedResponse<TelefonoDireccion>
					{
						Items = items,
						TotalRegistros = total,
						Pagina = pagina,
						TamañoPagina = tamañoPagina
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener clientes");
				return new ApiResponse<PaginatedResponse<TelefonoDireccion>>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<TelefonoDireccion>> CrearAsync(TelefonoDireccion cliente)
		{
			try
			{
				var maxId = await _context.Telefonos.MaxAsync(x => (int?)x.Id) ?? 0;
				cliente.Id = maxId + 1;

				_context.Telefonos.Add(cliente);
				await _context.SaveChangesAsync();

				return new ApiResponse<TelefonoDireccion>
				{
					Success = true,
					Message = "Cliente creado correctamente",
					Data = cliente
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al crear cliente");
				return new ApiResponse<TelefonoDireccion>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<TelefonoDireccion>> ActualizarAsync(int id, TelefonoDireccion cliente)
		{
			try
			{
				var existing = await _context.Telefonos.FindAsync(id);
				if (existing == null)
					return new ApiResponse<TelefonoDireccion>
					{
						Success = false,
						Message = "Cliente no encontrado"
					};

				existing.Telefono = cliente.Telefono;
				existing.Direccion = cliente.Direccion;
				existing.DireccionMayusculas = cliente.Direccion?.ToUpper();
				existing.Zona = cliente.Zona;
				existing.CuponDescuento = cliente.CuponDescuento;
				existing.NombreCupon = cliente.NombreCupon;

				await _context.SaveChangesAsync();

				return new ApiResponse<TelefonoDireccion>
				{
					Success = true,
					Message = "Cliente actualizado correctamente",
					Data = existing
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar cliente");
				return new ApiResponse<TelefonoDireccion>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<bool>> EliminarAsync(int id)
		{
			try
			{
				var existing = await _context.Telefonos.FindAsync(id);
				if (existing == null)
					return new ApiResponse<bool>
					{
						Success = false,
						Message = "Cliente no encontrado"
					};

				_context.Telefonos.Remove(existing);
				await _context.SaveChangesAsync();

				return new ApiResponse<bool>
				{
					Success = true,
					Message = "Cliente eliminado correctamente",
					Data = true
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar cliente");
				return new ApiResponse<bool>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message
				};
			}
		}
	}
}