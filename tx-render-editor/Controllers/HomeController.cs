using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tx_render_editor.Models;

namespace tx_render_editor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FullScreen()
        {
            return View();
        }

        public IActionResult Sidebar()
        {
            return View();
        }

        public IActionResult Modal()
        {
            return View();
        }

        public IActionResult SplitPanel()
        {
            return View();
        }

        public IActionResult TabbedView()
        {
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
