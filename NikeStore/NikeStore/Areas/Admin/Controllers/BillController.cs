using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BillController : Controller
    {
        public IActionResult Bill()
        {
            return View();
        }

        public IActionResult PaymentBill()
        {
            return View();
        }

        public IActionResult UnPaymentBill()
        {
            return View();
        }
    }
}
