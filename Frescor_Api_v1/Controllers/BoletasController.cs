using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Frescor_Api_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BoletasController : ControllerBase
    {
        private readonly IBoletaService _boletaService;
        private readonly ILogger<BoletasController> _logger;

        public BoletasController(IBoletaService boletaService, ILogger<BoletasController> logger)
        {
            _boletaService = boletaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Boleta>>>> GetByTelefono([FromQuery] string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return BadRequest(new ApiResponse<List<Boleta>> { Success = false, Message = "El teléfono es requerido" });

            var response = await _boletaService.GetBoletasByTelefonoAsync(telefono);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Boleta>>> Crear([FromBody] Boleta boleta)
        {
            _logger.LogInformation("Creando boleta para telefono {Telefono}", boleta.Telefono);
            var response = await _boletaService.CrearBoletaAsync(boleta);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}/pagar")]
        public async Task<ActionResult<ApiResponse<Boleta>>> MarcarPagada(int id)
        {
            _logger.LogInformation("Marcando boleta {Id} como pagada", id);
            var response = await _boletaService.MarcarPagadaAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Eliminar(int id)
        {
            _logger.LogInformation("Eliminando boleta {Id}", id);
            var response = await _boletaService.EliminarBoletaAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("deudores")]
        public async Task<ActionResult<ApiResponse<List<object>>>> GetDeudores()
        {
            var response = await _boletaService.GetDeudoresAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
