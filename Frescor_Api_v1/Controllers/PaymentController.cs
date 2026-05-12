using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Request;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.AspNetCore.Mvc;

namespace Frescor_Api_v1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly ILogger<PaymentController> _logger;
		private readonly IPaymentService _paymentService;

		public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
		{
			_logger = logger;
			_paymentService = paymentService;
		}


		/// <summary>
		/// Crea la preferencia de pago en Mercado Pago
		/// </summary>
		/// <param name="total">Monto total del pedido</param>
		/// <returns>URL para redirigir al checkout</returns>
		/// <response code="200">Preferencia creada correctamente</response>
		/// <response code="400">Error al crear la preferencia</response>
		[HttpPost("CreatePreference")]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<string>>> CreatePreference([FromBody] CreatePreferenceRequest request)
		{
			_logger.LogInformation("Creando preferencia de pago. Total: {Total}, OrderId: {OrderId}", request.Total, request.OrderId);

			var response = await _paymentService.CreatePreferenceAsync(request.Total, request.OrderId);

			if (response.Success)
			{
				_logger.LogInformation("Preferencia creada correctamente.");
				return Ok(response);
			}
			else
			{
				_logger.LogWarning("Error al crear preferencia: {Message}", response.Message);
				return BadRequest(response);
			}
		}

		/// <summary>
		/// Genera el comprobante PDF del pedido
		/// </summary>
		/// <param name="orderId">ID del pedido</param>
		/// <returns>Archivo PDF del comprobante</returns>
		/// <response code="200">Comprobante generado correctamente</response>
		/// <response code="400">Error al generar el comprobante</response>
		[HttpGet("GenerateReceipt")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GenerateReceipt(int orderId)
		{
			_logger.LogInformation("Generando comprobante PDF para pedido {OrderId}", orderId);

			var response = await _paymentService.GenerateReceiptAsync(orderId);

			if (response.Success)
			{
				_logger.LogInformation("Comprobante generado correctamente.");

				return File(
					response.Data,
					"application/pdf",
					$"Comprobante_Pedido_{orderId}.pdf");
			}
			else
			{
				_logger.LogWarning(
					"Error al generar comprobante: {Message}",
					response.Message);

				return BadRequest(new ApiResponse<string>
				{
					Success = false,
					Message = response.Message
				});
			}
		}

		[HttpPost("Webhook")]
		public async Task<IActionResult> MercadoPagoWebhook()
		{
			var id = Request.Query["id"].ToString();
			var topic = Request.Query["topic"].ToString();
			var type = Request.Query["type"].ToString();

			// Formato nuevo: query params id + type=payment
			// Formato viejo: query params id + topic=payment
			var paymentId = id;

			if (string.IsNullOrEmpty(paymentId))
				return Ok();

			var request = new MercadoPagoWebhookRequest
			{
				Type = string.IsNullOrEmpty(type) ? topic : type,
				Data = new WebhookData { Id = paymentId }
			};

			await _paymentService.ProcessWebhookAsync(request);
			return Ok();
		}
	}
}
