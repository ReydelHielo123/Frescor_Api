using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Request;

namespace Frescor_Api_v1.Interfaces
{
	public interface IPaymentService
	{
		Task<ApiResponse<string>> CreatePreferenceAsync(decimal total, int orderId);
		Task ProcessWebhookAsync(MercadoPagoWebhookRequest request);
		Task<ApiResponse<byte[]>> GenerateReceiptAsync(int orderId);
	}
}
