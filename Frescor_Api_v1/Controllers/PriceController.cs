using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frescor_Api_v1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class PriceController : Controller
	{
		private readonly ILogger<PriceController> _logger;
		private readonly IPriceService _priceService;

		public PriceController(IPriceService priceService, ILogger<PriceController> logger)
		{
			_priceService = priceService;
			_logger = logger;
		}


		// <summary>
		/// Obtiene todos los precios de las diferentes bolsas
		/// </summary>
		/// <returns>Lista de todos los precios</returns>
		/// <response code="200">Retorna la lista de precios</response>
		/// <response code="400">Error en la solicitud</response>
		[HttpGet]
		[ActionName("GetAllPrices")]
		[ProducesResponseType(typeof(ApiResponse<List<Precio>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<List<Precio>>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<List<Precio>>>> GetAllPrices()
		{
			_logger.LogInformation("Solicitando todos los precios");
			var response = await _priceService.GetAllPricesAsync();
			if (response.Success)
			{
				_logger.LogInformation("Precios obtenidos correctamente.");
				return Ok(response);
			}
			else
			{
				_logger.LogWarning("Error al recibir los precios: {Message}", response.Message);
				return BadRequest(response);
			}
		}

		// <summary>
		/// Actualiza el precio de una bolsa
		/// </summary>
		/// <returns>Precio actualizado</returns>
		/// <response code="200">Precio actualizado correctamente</response>
		/// <response code="400">Error en la solicitud</response>
		[HttpPut("{id}")]
		[ActionName("UpdatePrice")]
		[ProducesResponseType(typeof(ApiResponse<Precio>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<Precio>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<Precio>>> UpdatePrice(int id, [FromBody] Precio precio)
		{
			_logger.LogInformation("Actualizando precio ID: {Id}", id);
			var response = await _priceService.UpdatePriceAsync(id, precio);
			if (response.Success)
			{
				_logger.LogInformation("Precio actualizado correctamente.");
				return Ok(response);
			}
			else
			{
				_logger.LogWarning("Error al actualizar precio: {Message}", response.Message);
				return BadRequest(response);
			}
		}
	}
}
