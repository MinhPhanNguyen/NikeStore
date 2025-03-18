using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
