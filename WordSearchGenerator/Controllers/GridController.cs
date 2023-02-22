using Microsoft.AspNetCore.Mvc;

namespace WordSearchGenerator.Controllers
{
    public class GridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
