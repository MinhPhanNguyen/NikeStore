using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NikeStore.Areas.Admin.ApiController
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Order.OrderByDescending(p => p.Id).ToListAsync();
            return Ok(orders);
        }

        [HttpGet("done")]
        public async Task<ActionResult<IEnumerable<Order>>> GetDoneOrders()
        {
            var orders = await _context.Order.Where(p => p.Status == 0).OrderByDescending(p => p.Id).ToListAsync();
            return Ok(orders);
        }

        [HttpGet("waiting")]
        public async Task<ActionResult<IEnumerable<Order>>> GetWaitingOrders()
        {
            var orders = await _context.Order.Where(p => p.Status == 1).OrderByDescending(p => p.Id).ToListAsync();
            return Ok(orders);
        }

        [HttpGet("view/{orderCode}")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails(string orderCode)
        {
            var orderDetail = await _context.OrderDetail
                .Include(od => od.Product)
                .Include(od => od.Product.ProductType)
                .Include(od => od.Product.ProductColor)
                .Include(od => od.Product.ProductSize)
                .Where(od => od.OrderCode == orderCode)
                .ToListAsync();

            var shippingCost = await _context.Order.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (shippingCost == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            }

            return Ok(new { orderDetail, shippingCost = shippingCost.ShippingCost });
        }

        [HttpDelete("delete/{orderCode}")]
        public async Task<IActionResult> DeleteOrder(string orderCode)
        {
            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa đơn hàng thành công" });
        }
    }
}
