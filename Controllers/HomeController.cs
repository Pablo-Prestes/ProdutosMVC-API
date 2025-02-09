using Microsoft.AspNetCore.Mvc;

namespace ProdutosMvc.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
