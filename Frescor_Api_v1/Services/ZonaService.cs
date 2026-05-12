using Frescor_Api_v1.Data;
using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Frescor_Api_v1.Services
{
	public class ZonaService : IZonaService
	{
		private readonly AppDbContext _context;
		private readonly ILogger<ZonaService> _logger;

		public ZonaService(AppDbContext context, ILogger<ZonaService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<ApiResponse<List<Zona>>> ObtenerTodasAsync()
		{
			try
			{
				var zonas = await _context.Zonas
					.OrderBy(z => z.Zona1)
					.ToListAsync();

				return new ApiResponse<List<Zona>>
				{
					Success = true,
					Message = "Zonas obtenidas correctamente",
					Data = zonas
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener zonas");
				return new ApiResponse<List<Zona>>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<Zona>> CrearAsync(Zona zona)
		{
			try
			{
				var maxId = await _context.Zonas.MaxAsync(x => (int?)x.Id) ?? 0;
				zona.Id = maxId + 1;

				_context.Zonas.Add(zona);
				await _context.SaveChangesAsync();

				return new ApiResponse<Zona>
				{
					Success = true,
					Message = "Zona creada correctamente",
					Data = zona
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al crear zona");
				return new ApiResponse<Zona>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message,
					Data = null
				};
			}
		}

		public async Task<ApiResponse<Zona>> ActualizarAsync(int id, Zona zona)
		{
			try
			{
				var existing = await _context.Zonas.FindAsync(id);
				if (existing == null)
					return new ApiResponse<Zona>
					{
						Success = false,
						Message = "Zona no encontrada"
					};

				existing.Zona1 = zona.Zona1;
				existing.Barrio = zona.Barrio;

				await _context.SaveChangesAsync();

				return new ApiResponse<Zona>
				{
					Success = true,
					Message = "Zona actualizada correctamente",
					Data = existing
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar zona");
				return new ApiResponse<Zona>
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
				var existing = await _context.Zonas.FindAsync(id);
				if (existing == null)
					return new ApiResponse<bool>
					{
						Success = false,
						Message = "Zona no encontrada"
					};

				_context.Zonas.Remove(existing);
				await _context.SaveChangesAsync();

				return new ApiResponse<bool>
				{
					Success = true,
					Message = "Zona eliminada correctamente",
					Data = true
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar zona");
				return new ApiResponse<bool>
				{
					Success = false,
					Message = ex.InnerException?.Message ?? ex.Message
				};
			}
		}
	}
}