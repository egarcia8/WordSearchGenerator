using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WordSearchGenerator.Models;

namespace WordSearchGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {           
            return View();
        }        

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost("api/userinput")]
        public IActionResult UserInput(UserInput userInput) 
        {
            TempData["UserInput"] = JsonSerializer.Serialize(userInput);            
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}