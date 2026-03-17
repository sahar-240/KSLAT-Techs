using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMuseumData _data;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMuseumData data)
        {
            _logger = logger;
            _data = data;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                OpeningHours = _data.GetOpeningHours().ToList(),
                FAQs = _data.GetActiveFaqs().ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy() => View();

        public IActionResult Visit() => View();

        public IActionResult Contact() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}