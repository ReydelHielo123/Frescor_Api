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
		[FromQuery] int tamañoPagina = 50)
		{
			_logger.LogInformation("Filtrando pedidos");
			var response = await _orderService.FiltrarPedidosAsync(
				desde, hasta, direccion, zona, formaPago, estado, pagina, tamañoPagina);
			if (response.Success)
				return Ok(response);
			else
				return BadRequest(response);
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
