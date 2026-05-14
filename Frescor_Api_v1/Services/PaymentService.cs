using Frescor_Api_v1.Data;
using Frescor_Api_v1.Interfaces;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Request;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.Drawing;

namespace Frescor_Api_v1.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger<PaymentService> _logger;
		private readonly AppDbContext _context;

		public PaymentService(IConfiguration configuration, ILogger<PaymentService> logger, AppDbContext context)
		{
			_configuration = configuration;
			_logger = logger;
			_context = context;
		}

		public async Task<ApiResponse<string>> CreatePreferenceAsync(decimal total, int orderId)
		{
			try
			{
				var token = _configuration["MercadoPago:AccessToken"];
				var frontendBaseUrl = _configuration["Frontend:BaseUrl"];

				_logger.LogInformation("Token MercadoPago leído: {TieneToken}", !string.IsNullOrEmpty(token));

				MercadoPagoConfig.AccessToken = token!;

				var request = new PreferenceRequest
				{
					Items = new List<PreferenceItemRequest>
					{
						new PreferenceItemRequest
						{
							Title = $"Pedido Frescor #{orderId}",
							Quantity = 1,
							CurrencyId = "ARS",
							UnitPrice = total
						}
					},
					ExternalReference = orderId.ToString(),
					NotificationUrl = "https://frescorapi-production.up.railway.app/api/payment/Webhook",
					AutoReturn = "approved",
					BackUrls = new PreferenceBackUrlsRequest
					{
						Success = "https://frescor-app-ulz4.vercel.app/#/payment-result",
						Failure = "https://frescor-app-ulz4.vercel.app/#/payment-result",
						Pending = "https://frescor-app-ulz4.vercel.app/#/payment-result"
					},
				};

				var client = new PreferenceClient();
				var preference = await client.CreateAsync(request);

				return new ApiResponse<string>
				{
					Success = true,
					Message = "Preferencia creada correctamente",
					Data = preference.InitPoint!
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al crear preferencia de Mercado Pago");
				return new ApiResponse<string>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task ProcessWebhookAsync(MercadoPagoWebhookRequest request)
		{
			try
			{
				if (request.Type != "payment")
					return;

				var client = new PaymentClient();
				var payment = await client.GetAsync(long.Parse(request.Data.Id));

				var orderId = int.Parse(payment.ExternalReference);

				var pedido = await _context.Pedido2
					.FirstOrDefaultAsync(x => x.Id == orderId);

				if (pedido == null)
					return;

				pedido.PaymentId = payment.Id.ToString();

				switch (payment.Status)
				{
					case "approved":
						pedido.Estado = "Pagado";
						break;

					case "pending":
					case "in_process":
						pedido.Estado = "PagoPendiente";
						break;

					case "rejected":
					case "cancelled":
						pedido.Estado = "PagoRechazado";
						break;
				}

				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error procesando webhook MP");
			}
		}

		public async Task<ApiResponse<byte[]>> GenerateReceiptAsync(int orderId)
		{
			try
			{
				var pedido = await _context.Pedido2
					.FirstOrDefaultAsync(x => x.Id == orderId);
				if (pedido == null)
				{
					return new ApiResponse<byte[]>
					{
						Success = false,
						Message = "Pedido no encontrado"
					};
				}
				using var stream = new MemoryStream();
				var document = new PdfDocument();
				var page = document.AddPage();
				var gfx = XGraphics.FromPdfPage(page);

				var titleFont = new XFont("Noto Sans", 18, XFontStyleEx.Bold);
				var normalFont = new XFont("Noto Sans", 12, XFontStyleEx.Regular);

				int y = 40;

				gfx.DrawString("FRESCOR", titleFont, XBrushes.Black, 40, y);
				y += 40;

				gfx.DrawString($"Pedido N°: {pedido.Id}", normalFont, XBrushes.Black, 40, y);
				y += 25;

				gfx.DrawString($"Fecha: {pedido.Fecha_Carga:dd/MM/yyyy HH:mm}", normalFont, XBrushes.Black, 40, y);
				y += 25;

				gfx.DrawString($"Direccion: {pedido.Direccion}", normalFont, XBrushes.Black, 40, y);
				y += 25;

				gfx.DrawString($"Forma de pago: {pedido.FormaPago}", normalFont, XBrushes.Black, 40, y);
				y += 25;

				gfx.DrawString($"Estado: {pedido.Estado}", normalFont, XBrushes.Black, 40, y);
				y += 25;

				gfx.DrawString($"Total: ${pedido.Precio_Total}", normalFont, XBrushes.Black, 40, y);
				y += 25;

				gfx.DrawString($"10kg: {pedido.Kg10_5}", normalFont, XBrushes.Black, 40, y);
				y += 20;

				gfx.DrawString($"2,5kg: {pedido.Kg2_5}", normalFont, XBrushes.Black, 40, y);
				y += 20;

				gfx.DrawString($"1,5kg: {pedido.Kg1_5}", normalFont, XBrushes.Black, 40, y);
				y += 20;

				gfx.DrawString($"Triturado: {pedido.Triturado}", normalFont, XBrushes.Black, 40, y);

				document.Save(stream, false);

				return new ApiResponse<byte[]>
				{
					Success = true,
					Data = stream.ToArray(),
					Message = "Comprobante generado correctamente"
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al generar comprobante PDF");
				return new ApiResponse<byte[]>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}
