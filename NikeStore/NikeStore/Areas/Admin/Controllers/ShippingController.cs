using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingController : Controller
    {
        public readonly DataContext _context;

        public ShippingController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Shipping()
        {
            var shippingList = await _context.Shipping.ToListAsync();
            return View(shippingList);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StoreShipping(Shipping shipping, string phuong, string quan, string tinh, decimal price)
        {
            shipping.City = tinh;
            shipping.District = quan;
            shipping.Ward = phuong;
            shipping.Price = price;

            try
            {
                var existingShipping = await _context.Shipping.AnyAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

                if(existingShipping)
                {
                    return Ok(new {duplicate = true, message = "Dữ liệu không trùng lặp" });
                }
                _context.Shipping.Add(shipping);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm vận chuyển thành công" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Chương trình gặp lỗi trong khi thêm vận chuyển");
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            Shipping shipping = await _context.Shipping.FindAsync(Id);

            _context.Shipping.Remove(shipping);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successfully";
            return RedirectToAction("Shipping");
        }
    }
}
