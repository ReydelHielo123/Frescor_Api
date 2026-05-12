using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Frescor_Api_v1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class CuponesController : ControllerBase
	{
		private readonly ICuponService _cuponService;
		private readonly ILogger<CuponesController> _logger;

		public CuponesController(ICuponService cuponService, ILogger<CuponesController> logger)
		{
			_cuponService = cuponService;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene todos los cupones con filtros y paginado
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<PaginatedResponse<Cupone>>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<PaginatedResponse<Cupone>>>> GetTodos(
			[FromQuery] string? codigo,
			[FromQuery] string? descripcion,
			[FromQuery] decimal? porcentaje,
			[FromQuery] int pagina = 1,
			[FromQuery] int tamañoPagina = 20)
		{
			_logger.LogInformation("Solicitando cupones");
			var response = await _cuponService.ObtenerTodosAsync(codigo, descripcion, porcentaje, pagina, tamañoPagina);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Crea un nuevo cupón
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<Cupone>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<Cupone>>> Crear([FromBody] Cupone cupon)
		{
			_logger.LogInformation("Creando nuevo cupón");
			var response = await _cuponService.CrearAsync(cupon);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Actualiza un cupón existente
		/// </summary>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ApiResponse<Cupone>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<Cupone>>> Actualizar(int id, [FromBody] Cupone cupon)
		{
			_logger.LogInformation("Actualizando cupón ID: {Id}", id);
			var response = await _cuponService.ActualizarAsync(id, cupon);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Elimina un cupón
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<bool>>> Eliminar(int id)
		{
			_logger.LogInformation("Eliminando cupón ID: {Id}", id);
			var response = await _cuponService.EliminarAsync(id);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Obtiene un cupón por ID
		/// </summary>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApiResponse<Cupone>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<Cupone>>> GetById(int id)
		{
			_logger.LogInformation("Solicitando cupón ID: {Id}", id);
			var response = await _cuponService.ObtenerPorIdAsync(id);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}
	}
}