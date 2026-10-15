using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Frescor_Api_v1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]

	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly ILogger<OrdersController> _logger;

		public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
		{
			_orderService = orderService;
			_logger = logger;
		}

		// <summary>
		/// Obtiene todos los pedidos realizados
		/// </summary>
		/// <returns>Lista de todos los pedidos</returns>
		/// <response code="200">Retorna la lista de pedidos</response>
		/// <response code="400">Error en la solicitud</response>
		[HttpGet]
		[ActionName("GetAllOrders")]
		[ProducesResponseType(typeof(ApiResponse<List<Order>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<List<Order>>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<List<Order>>>> GetAllOrders()
		{
			_logger.LogInformation("Solicitando todos los pedidos");
			var response = await _orderService.GetAllOrdersAsync();
			if (response.Success)
			{
				_logger.LogInformation("Pedidos Obtenidos correctamente.");
				return Ok(response);
			}
			else
			{
				_logger.LogWarning("Error al recibir los pedidos: {Message}", response.Message);
				return BadRequest(response);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ApiResponse<Pedido2>>> GetOrderById(int id)
		{
			var response = await _orderService.GetOrderByIdAsync(id);
			if (response.Success) return Ok(response);
			return BadRequest(response);
		}

		[HttpPost]
		[ActionName("InsertOrder")]
		[ProducesResponseType(typeof(ApiResponse<List<Pedido2>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<List<Pedido2>>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<Pedido2>>> InsertOrder([FromBody] Pedido2 order)
		{
			_logger.LogInformation("Insertar pedido");
			var response = await _orderService.InsertOrderAsync(order);

			if (response.Success)
			{
				_logger.LogInformation("Pedido insertado exitosamente");
				return Ok(response);
			}
			else
			{
				_logger.LogWarning("Error al insertar los pedidos: {Message}", response.Message);
				return BadRequest();
			}
		}

		[HttpGet("filtrar")]
		public async Task<ActionResult<ApiResponse<PaginatedResponse<Pedido2>>>> FiltrarPedidos(
		[FromQuery] DateTime? desde,
		[FromQuery] DateTime? hasta,
		[FromQuery] string? direccion,
		[FromQuery] int? zona,
		[FromQuery] string? formaPago,
		[FromQuery] string? estado,
		[FromQuery] int pagina = 1,
		[FromQuery] int tamañoPagina = 50,
		[FromQuery] string? telefono = null)
		{
			_logger.LogInformation("Filtrando pedidos");
			var response = await _orderService.FiltrarPedidosAsync(
				desde, hasta, direccion, zona, formaPago, estado, pagina, tamañoPagina, telefono);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}

        /// <summary>Pedidos de un teléfono durante los últimos 14 días calendario de Argentina.</summary>
        [HttpGet("recientes")]
        [ProducesResponseType(typeof(ApiResponse<PedidosRecientesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidosRecientesResponse>>> GetPedidosRecientes(
            [FromQuery] string? telefono, [FromQuery] int pagina = 1, [FromQuery] int tamañoPagina = 10)
        {
            var normalizado = System.Text.RegularExpressions.Regex.Replace(telefono ?? "", @"[\s()+.\-]", "");
            if (!System.Text.RegularExpressions.Regex.IsMatch(normalizado, @"^[0-9]{10,15}$") ||
                pagina < 1 || pagina > 100000 || tamañoPagina < 1 || tamañoPagina > 50)
                return BadRequest(new ApiResponse<PedidosRecientesResponse>
                { Success = false, Message = "Ingresá un teléfono de 10 a 15 dígitos y una paginación válida (1 a 50 pedidos por página)." });

            var response = await _orderService.GetPedidosRecientesAsync(normalizado, pagina, tamañoPagina);
            return response.Success ? Ok(response) : StatusCode(StatusCodes.Status500InternalServerError, response);
        }

		[HttpGet("metricas")]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<object>>> GetMetricas()
		{
			_logger.LogInformation("Obteniendo métricas del dashboard");
			var response = await _orderService.GetMetricasAsync();
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
		}
	}
}
