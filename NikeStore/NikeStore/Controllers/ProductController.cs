using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;

namespace NikeStore.Controllers
{
    [Route("Product/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private UserManager<AppUserModel> _userManager;

        public ProductController(DataContext dataContext, UserManager<AppUserModel> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dataContext.Product.Include(p => p.ProductCategory).Include(p => p.Images).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(long id)
        {
            if (id <= 0) return RedirectToAction("Index");
            var productById = _dataContext.Product.Where(p => p.ProductID == id).Include(p => p.Images).FirstOrDefault();
            return View(productById);
        }

        [Authorize]
        public async Task<IActionResult> WishList()
        {
            var wishlist_product = await (from w in _dataContext.WishList
                                          join p in _dataContext.Product on w.ProductId equals p.ProductID
                                          select new { Product = p, WishList = w })
                                          .ToListAsync();

            return View(wishlist_product);
        }

        [Authorize]
        public async Task<IActionResult> DeleteWishList(int Id)
        {
            WishList wishList = await _dataContext.WishList.FindAsync(Id);
            _dataContext.WishList.Remove(wishList);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa sản phẩm yêu thích thành công";
            return RedirectToAction("WishList");
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList([FromBody] long Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new { success = false, message = "ID sản phẩm không hợp lệ!" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Người dùng chưa đăng nhập" });
            }

            var existingWish = await _dataContext.WishList
                .FirstOrDefaultAsync(w => w.ProductId == Id && w.UserId == user.Id);

            if (existingWish != null)
            {
                return BadRequest(new { success = false, message = "Sản phẩm đã có trong danh sách yêu thích" });
            }

            var wishListItem = new WishList
            {
                ProductId = Id,
                UserId = user.Id
            };

            _dataContext.Add(wishListItem);

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm danh sách yêu thích thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi server: " + ex.Message);
                return StatusCode(500, "Thêm danh sách yêu thích thất bại");
            }
        }
    }
}