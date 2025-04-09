using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NikeStore.Models;
using NikeStore.Repository;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashBoardController : Controller
    {
        private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "VisitorTimestamps";
        public DashBoardController(DataContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public IActionResult Index()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            decimal revenueToday = _context.Order
                .Where(o => o.CreateDate.Date == today)
                .Sum(o => (decimal?)o.ShippingCost - o.Discount) ?? 0;

            decimal revenueYesterday = _context.Order
                .Where(o => o.CreateDate.Date == yesterday)
                .Sum(o => (decimal?)o.ShippingCost - o.Discount) ?? 0;

            int productsSoldToday = (from od in _context.OrderDetail
                                     join o in _context.Order on od.OrderCode equals o.OrderCode
                                     where o.CreateDate.Date == today
                                     select od.Quantity).Sum();

            int productsSoldYesterday = (from od in _context.OrderDetail
                                         join o in _context.Order on od.OrderCode equals o.OrderCode
                                         where o.CreateDate.Date == yesterday
                                         select od.Quantity).Sum();

            int totalOrderToday = _context.Order.Count(o => o.CreateDate.Date == today);
            int totalOrderYesterday = _context.Order.Count(o => o.CreateDate.Date == yesterday);
            int totalWebsiteVisits = HttpContext.Session.GetInt32("TotalWebsiteVisits") ?? 0;

            ViewBag.RevenueGrowth = revenueYesterday > 0 ? ((revenueToday - revenueYesterday) / revenueYesterday * 100).ToString("0.##") : "100";
            ViewBag.ProductGrowth = productsSoldYesterday > 0 ? ((productsSoldToday - productsSoldYesterday) / (double)productsSoldYesterday * 100).ToString("0.##") : "100";
            ViewBag.OrderGrowth = totalOrderYesterday > 0 ? ((totalOrderToday - totalOrderYesterday) / (double)totalOrderYesterday * 100).ToString("0.##") : "100";
            ViewBag.TotalRevenueToday = revenueToday.ToString("N0") + " VND";
            ViewBag.TotalProductsSold = productsSoldToday;
            ViewBag.TotalOrder = totalOrderToday;
            ViewBag.TotalWebsiteVisits = totalWebsiteVisits;

            return View();
        }

        public IActionResult TrackWebsiteVisit()
        {
            string sessionKey = "TotalWebsiteVisits";

            // Tăng lượt truy cập trong Session
            int currentVisits = HttpContext.Session.GetInt32(sessionKey) ?? 0;
            HttpContext.Session.SetInt32(sessionKey, currentVisits + 1);

            return Ok();
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
