using Frescor_Api_v1.Data;
using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Frescor_Api_v1.Services
{
	public class CuponService : ICuponService
	{
		private readonly AppDbContext _context;
		private readonly ILogger<CuponService> _logger;

		public CuponService(AppDbContext context, ILogger<CuponService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<ApiResponse<PaginatedResponse<Cupone>>> ObtenerTodosAsync(
			string? codigo, string? descripcion, decimal? porcentaje, int pagina, int tamañoPagina)
		{
			try
			{
				var query = _context.Cupones.AsQueryable();

				if (!string.IsNullOrEmpty(codigo))
					query = query.Where(x => x.Codigo.Contains(codigo));

				if (!string.IsNullOrEmpty(descripcion))
					query = query.Where(x => x.Descripcion != null && x.Descripcion.Contains(descripcion));

				if (porcentaje.HasValue)
					query = query.Where(x => x.Porcentaje == porcentaje.Value);

				var total = await query.CountAsync();

				var items = await query
					.OrderBy(x => x.Codigo)
					.Skip((pagina - 1) * tamañoPagina)
					.Take(tamañoPagina)
					.ToListAsync();

				return new ApiResponse<PaginatedResponse<Cupone>>
				{
					Success = true,
					Message = "Cupones obtenidos correctamente",
					Data = new PaginatedResponse<Cupone>
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
				_logger.LogError(ex, "Error al obtener cupones");
				return new ApiResponse<PaginatedResponse<Cupone>>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<Cupone>> CrearAsync(Cupone cupon)
		{
			try
			{
				cupon.Id = 0; // EF lo ignora si hay identity
				_context.Cupones.Add(cupon);
				await _context.SaveChangesAsync();

				return new ApiResponse<Cupone>
				{
					Success = true,
					Message = "Cupón creado correctamente",
					Data = cupon
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al crear cupón");
				return new ApiResponse<Cupone>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message,
					Data = null
				};
			}
		}
		public async Task<ApiResponse<Cupone>> ActualizarAsync(int id, Cupone cupon)
		{
			try
			{
				var existing = await _context.Cupones.FindAsync(id);
				if (existing == null)
					return new ApiResponse<Cupone>
					{
						Success = false,
						Message = "Cupón no encontrado"
					};

				existing.Codigo = cupon.Codigo;
				existing.Porcentaje = cupon.Porcentaje;
				existing.Descripcion = cupon.Descripcion;

				await _context.SaveChangesAsync();

				return new ApiResponse<Cupone>
				{
					Success = true,
					Message = "Cupón actualizado correctamente",
					Data = existing
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar cupón");
				return new ApiResponse<Cupone>
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
				var existing = await _context.Cupones.FindAsync(id);
				if (existing == null)
					return new ApiResponse<bool>
					{
						Success = false,
						Message = "Cupón no encontrado"
					};

				_context.Cupones.Remove(existing);
				await _context.SaveChangesAsync();

				return new ApiResponse<bool>
				{
					Success = true,
					Message = "Cupón eliminado correctamente",
					Data = true
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar cupón");
				return new ApiResponse<bool>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message
				};
			}
		}
		public async Task<ApiResponse<Cupone>> ObtenerPorIdAsync(int id)
		{
			try
			{
				var cupon = await _context.Cupones.FindAsync(id);
				if (cupon == null)
					return new ApiResponse<Cupone>
					{
						Success = false,
						Message = "Cupón no encontrado"
					};

				return new ApiResponse<Cupone>
				{
					Success = true,
					Message = "Cupón obtenido correctamente",
					Data = cupon
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener cupón por ID");
				return new ApiResponse<Cupone>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}