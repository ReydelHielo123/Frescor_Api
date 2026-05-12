namespace Frescor_Api_v1.Models.Request
{
	public class MercadoPagoWebhookRequest
	{
		public string Type { get; set; }
		public WebhookData Data { get; set; }
	}

	public class WebhookData
	{
		public string Id { get; set; }
	}
}
