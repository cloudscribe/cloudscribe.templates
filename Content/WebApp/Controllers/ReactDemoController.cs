using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ReactDemoController : Controller
    {
        public ReactDemoController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Demo1()
        {
            return View("Demo1");
        }

        public IActionResult Demo2()
        {
            return View("Demo2");
        }
    }
}
