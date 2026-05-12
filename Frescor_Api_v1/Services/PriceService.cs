using Frescor_Api_v1.Data;
using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace Frescor_Api_v1.Service
{
	public class PriceService : IPriceService
	{
		private readonly AppDbContext _context;
		private readonly ILogger<PriceService> _logger;

		public PriceService(AppDbContext context, ILogger<PriceService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<ApiResponse<List<Precio>>> GetAllPricesAsync()
		{
			try
			{
				var prices = await _context.Precios.ToListAsync();
				return new ApiResponse<List<Precio>>
				{
					Success = true,
					Message = "Todos los precios devueltos exitosamente",
					Data = prices
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al devolver todos los precios");
				return new ApiResponse<List<Precio>>
				{
					Success = false,
					Message = ex.Message,
					Data = null
				};
			}
		}
		public async Task<ApiResponse<Precio>> UpdatePriceAsync(int id, Precio precio)
		{
			try
			{
				var existing = await _context.Precios.FindAsync(id);
				if (existing == null)
				{
					return new ApiResponse<Precio>
					{
						Success = false,
						Message = "Precio no encontrado"
					};
				}

				existing.Precio1 = precio.Precio1;
				await _context.SaveChangesAsync();

				return new ApiResponse<Precio>
				{
					Success = true,
					Message = "Precio actualizado correctamente",
					Data = existing
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar precio");
				return new ApiResponse<Precio>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}
