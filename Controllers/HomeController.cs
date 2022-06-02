using Laba2FilmsBD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Laba2FilmsBD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly static RequestsController request = new RequestsController();


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? Rnumber, bool? val)
        {
            ViewBag.LastRequest = null;
            ViewBag.LastValue = null;
            ViewBag.Fans = null;
            if (Rnumber == null) return View();
            if (Rnumber == 8)
            {
                ViewBag.LastRequest = "8";
                if (val == false) ViewBag.LastValue = "мінімальним";
                else ViewBag.LastValue = "максимальним";
                ViewBag.Fans = request.Request8((bool)val);
                return View();
            }
            ViewBag.ErrorMessage = "Unknown Request";         

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}