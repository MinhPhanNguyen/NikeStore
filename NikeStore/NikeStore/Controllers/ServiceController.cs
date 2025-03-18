using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Repository;

namespace NikeStore.Controllers
{
    public class ServiceController : Controller
    {
        private readonly DataContext _context;
        public ServiceController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Service(int Id)
        {
            if (Id <= 0) return RedirectToAction("Index");
            var serviceById = _context.ServiceType
                .FirstOrDefault(p => p.ServiceTypeID == Id);
            if (serviceById == null) return NotFound();
            return View(serviceById);
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Repairing()
        {
            return View();
        }
    }
}
