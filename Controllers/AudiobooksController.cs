using Microsoft.AspNetCore.Mvc;

namespace BookCatalogApp.Controllers
{
    public class AudiobooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}