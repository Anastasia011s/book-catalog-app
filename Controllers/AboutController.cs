using Microsoft.AspNetCore.Mvc;

namespace BookCatalogApp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}