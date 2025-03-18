using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WarehouseController : Controller
    {
        public IActionResult WarehouseExporting()
        {
            return View();
        }

        public IActionResult WarehouseImporting()
        {
            return View();
        }

        public IActionResult WarehouseInventory()
        {
            return View();
        }

        public IActionResult WarehouseProvider()
        {
            return View();
        }
    }
}
