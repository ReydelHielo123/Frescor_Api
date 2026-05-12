using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Frescor_Api_v1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class ZonasController : ControllerBase
	{
		private readonly IZonaService _zonaService;
		private readonly ILogger<ZonasController> _logger;

		public ZonasController(IZonaService zonaService, ILogger<ZonasController> logger)
		{
			_zonaService = zonaService;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene todas las zonas
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<List<Zona>>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<List<Zona>>>> GetTodas()
		{
			_logger.LogInformation("Solicitando todas las zonas");
			var response = await _zonaService.ObtenerTodasAsync();
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Crea una nueva zona
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<Zona>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<Zona>>> Crear([FromBody] Zona zona)
		{
			_logger.LogInformation("Creando nueva zona");
			var response = await _zonaService.CrearAsync(zona);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Actualiza una zona existente
		/// </summary>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ApiResponse<Zona>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<Zona>>> Actualizar(int id, [FromBody] Zona zona)
		{
			_logger.LogInformation("Actualizando zona ID: {Id}", id);
			var response = await _zonaService.ActualizarAsync(id, zona);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Elimina una zona
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<bool>>> Eliminar(int id)
		{
			_logger.LogInformation("Eliminando zona ID: {Id}", id);
			var response = await _zonaService.EliminarAsync(id);
			if (response.Success)
			{
				_logger.LogInformation("Zona eliminada correctamente.");
				return Ok(response);
			}
			else
			{
				_logger.LogWarning("Error al eliminar zona: {Message}", response.Message);
				return BadRequest(response);
			}
		}
	}
}