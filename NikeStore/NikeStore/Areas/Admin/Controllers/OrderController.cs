using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Order()
        {
            var order = await _context.Order.OrderByDescending(p => p.Id).ToListAsync();
            return View(order);
        }

        public async Task<IActionResult> DoneOrder()
        {
            var order = await _context.Order.OrderByDescending(p => p.Id).Where(p => p.Status == 0).ToListAsync();
            return View(order);
        }

        public async Task<IActionResult> WaitingOrder()
        {
            var order = await _context.Order.OrderByDescending(p => p.Id).Where(p => p.Status == 1).ToListAsync();
            return View(order);
        }

        public async Task<IActionResult> ViewOrder(string orderCode)
        {
            var orderDetail = await _context.OrderDetail
                .Include(od => od.Product)
                .Include(od => od.Product.ProductType)
                .Include(od => od.Product.ProductColor)
                .Include(od => od.Product.ProductSize)
                .Include(od => od.Product.Promotion)
                .Where(od => od.OrderCode == orderCode)
                .ToListAsync();

            return View(orderDetail);
        }

        public async Task<IActionResult> Delete(string orderCode)
        {
            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (order == null)
            {
                TempData["error"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Order");
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            TempData["success"] = "Delete order successfully";
            return RedirectToAction("Order");
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string orderCode, int status)
        {
            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            _context.Order.Update(order);
            await _context.SaveChangesAsync();
            TempData["success"] = "Update order successfully";
            return RedirectToAction("Order");
        }
    }
}
