using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Frescor_Api_v1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class ClientDataController : Controller
	{
		private readonly ITelefonoDireccionService _telefonoDireccionService;
		private readonly ILogger<OrdersController> _logger;

		public ClientDataController(ITelefonoDireccionService telefonoDireccionService, ILogger<OrdersController> logger)
		{
			_telefonoDireccionService = telefonoDireccionService;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene las direcciones en base al telefono
		/// </summary>
		[HttpGet]
		[ActionName("GetDireccionesTelefono")]
		[ProducesResponseType(typeof(ApiResponse<List<TelefonoDireccion>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<List<TelefonoDireccion>>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<List<TelefonoDireccion>>>> GetDireccionesTelefono([FromQuery] string telefono)
		{
			_logger.LogInformation("Solicitando direcciones del telefono: {Telefono}", telefono);
			var response = await _telefonoDireccionService.ObtenerDireccionesPorTelefonoAsync(telefono);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Obtiene todos los clientes con paginado y filtro
		/// </summary>
		[HttpGet("todos")]
		[ProducesResponseType(typeof(ApiResponse<PaginatedResponse<TelefonoDireccion>>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<PaginatedResponse<TelefonoDireccion>>>> GetTodos(
			[FromQuery] string? telefono,
			[FromQuery] string? direccion,
			[FromQuery] int? zona,
			[FromQuery] int pagina = 1,
			[FromQuery] int tamañoPagina = 20)
		{
			_logger.LogInformation("Solicitando todos los clientes");
			var response = await _telefonoDireccionService.ObtenerTodosAsync(telefono, direccion, zona, pagina, tamañoPagina);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Crea un nuevo cliente
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<TelefonoDireccion>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<TelefonoDireccion>>> Crear([FromBody] TelefonoDireccion cliente)
		{
			_logger.LogInformation("Creando nuevo cliente");
			var response = await _telefonoDireccionService.CrearAsync(cliente);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Actualiza un cliente existente
		/// </summary>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ApiResponse<TelefonoDireccion>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<TelefonoDireccion>>> Actualizar(int id, [FromBody] TelefonoDireccion cliente)
		{
			_logger.LogInformation("Actualizando cliente ID: {Id}", id);
			var response = await _telefonoDireccionService.ActualizarAsync(id, cliente);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

		/// <summary>
		/// Elimina un cliente
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<bool>>> Eliminar(int id)
		{
			_logger.LogInformation("Eliminando cliente ID: {Id}", id);
			var response = await _telefonoDireccionService.EliminarAsync(id);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}
	}
}