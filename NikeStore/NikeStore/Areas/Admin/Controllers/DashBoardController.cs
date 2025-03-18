using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashBoardController : Controller
    {
        private readonly DataContext _context;
        public DashBoardController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RevenueChart()
        {
            return View();
        }

        public IActionResult ProductChart()
        {
            return View();
        }

        public IActionResult OrderChart()
        {
            return View();
        }

        public IActionResult AccessWebStatistic()
        {
            return View();
        }
    }
}
