using Microsoft.AspNetCore.Mvc;

namespace Frescor_Api_v1.Controllers
{
	public class OrdersController : ControllerBase
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
