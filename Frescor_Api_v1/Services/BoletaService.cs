using Frescor_Api_v1.Data;
using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Frescor_Api_v1.Services
{
    public class BoletaService : IBoletaService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BoletaService> _logger;

        public BoletaService(AppDbContext context, ILogger<BoletaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<List<Boleta>>> GetBoletasByTelefonoAsync(string telefono)
        {
            try
            {
                var boletas = await _context.Boletas
                    .Include(b => b.Items)
                    .Where(b => b.Telefono == telefono)
                    .OrderByDescending(b => b.FechaBoleta)
                    .ToListAsync();

                return new ApiResponse<List<Boleta>> { Success = true, Data = boletas };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener boletas para telefono {Telefono}", telefono);
                return new ApiResponse<List<Boleta>> { Success = false, Message = ex.Message, Data = null };
            }
        }

        public async Task<ApiResponse<Boleta>> CrearBoletaAsync(Boleta boleta)
        {
            try
            {
                if (boleta.PedidoId.HasValue)
                {
                    var duplicada = await _context.Boletas
                        .AnyAsync(b => b.PedidoId == boleta.PedidoId);
                    if (duplicada)
                        return new ApiResponse<Boleta>
                        {
                            Success = false,
                            Message = "Ya existe una boleta generada para este pedido."
                        };
                }

                boleta.FechaCarga = DateTime.UtcNow;
                boleta.Estado = "Pendiente";
                boleta.ImporteTotal = boleta.Items.Sum(i => i.Subtotal);

                _context.Boletas.Add(boleta);
                await _context.SaveChangesAsync();

                return new ApiResponse<Boleta> { Success = true, Data = boleta };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear boleta");
                return new ApiResponse<Boleta> { Success = false, Message = ex.InnerException?.Message ?? ex.Message, Data = null };
            }
        }

        public async Task<ApiResponse<Boleta>> MarcarPagadaAsync(int id)
        {
            try
            {
                var boleta = await _context.Boletas.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == id);
                if (boleta == null)
                    return new ApiResponse<Boleta> { Success = false, Message = "Boleta no encontrada", Data = null };

                boleta.Estado = "Pagada";
                await _context.SaveChangesAsync();

                return new ApiResponse<Boleta> { Success = true, Data = boleta };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar boleta como pagada");
                return new ApiResponse<Boleta> { Success = false, Message = ex.Message, Data = null };
            }
        }

        public async Task<ApiResponse<bool>> EliminarBoletaAsync(int id)
        {
            try
            {
                var boleta = await _context.Boletas.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == id);
                if (boleta == null)
                    return new ApiResponse<bool> { Success = false, Message = "Boleta no encontrada", Data = false };

                _context.BoletaItems.RemoveRange(boleta.Items);
                _context.Boletas.Remove(boleta);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar boleta {Id}", id);
                return new ApiResponse<bool> { Success = false, Message = ex.Message, Data = false };
            }
        }

        public async Task<ApiResponse<List<object>>> GetDeudoresAsync()
        {
            try
            {
                var grupos = await _context.Boletas
                    .Where(b => b.Estado == "Pendiente")
                    .GroupBy(b => b.Telefono)
                    .Select(g => new
                    {
                        Telefono = g.Key,
                        CantidadBoletas = g.Count(),
                        TotalAdeudado = g.Sum(b => b.ImporteConDescuento.HasValue
                            ? b.ImporteConDescuento.Value
                            : b.ImporteTotal),
                        FechaMasAntigua = g.Min(b => b.FechaBoleta)
                    })
                    .ToListAsync();

                var deudores = grupos.Select(g => (object)g).ToList();

                return new ApiResponse<List<object>> { Success = true, Data = deudores };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener deudores");
                return new ApiResponse<List<object>> { Success = false, Message = ex.Message, Data = null };
            }
        }
    }
}
