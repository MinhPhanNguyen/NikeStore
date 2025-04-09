using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Models.ViewModels;
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

        public async Task<IActionResult> Index(string sort_by = "", decimal? startprice = null, decimal? endprice = null)
        {
            IQueryable<Product> productsQuery = _dataContext.Product
                .Include(p => p.ProductCategory)
                .Include(p => p.Images);

            if (startprice.HasValue && endprice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= startprice.Value && p.Price <= endprice.Value);
            }

            switch (sort_by)
            {
                case "price_increase":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "price_decrease":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                case "price_newest":
                    productsQuery = productsQuery.OrderByDescending(p => p.CreatedAt);
                    break;
                case "price_hotest":
                    productsQuery = productsQuery.Where(p => p.IsHot).OrderByDescending(p => p.CreatedAt);
                    break;
                default:
                    productsQuery = productsQuery.OrderByDescending(p => p.ProductID);
                    break;
            }

            var products = await productsQuery.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(long id)
        {
            if (id <= 0) return RedirectToAction("Index");

            var productById = _dataContext.Product
                .Where(p => p.ProductID == id)
                .Include(p => p.Images)
                .Include(p => p.ProductCategory)
                .FirstOrDefault();

            if (productById == null)
                return NotFound();

            var relatedProducts = _dataContext.Product
                .Where(p => p.CategoryID == productById.CategoryID && p.ProductID != id)
                .Include(p => p.Images)
                .Include(p => p.ProductCategory)
                .Take(5)
                .ToList();  

            var viewModel = new ProductDetailViewModel
            {
                Product = productById,
                RelatedProducts = relatedProducts
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> WishList()
        {
            var wishlist_product = await (from w in _dataContext.WishList
                                          join p in _dataContext.Product on w.ProductId equals p.ProductID
                                          select new WishListViewModel
                                          {
                                              ProductID = p.ProductID,
                                              Name = p.Name,
                                              ImageUrl = p.Images.FirstOrDefault().ImageUrl ?? "default.jpg",
                                              CategoryName = p.ProductCategory.CategoryName,
                                              Price = p.Price
                                          })
                                          .ToListAsync();

            return View(wishlist_product);
        }


        [Authorize]
        public async Task<IActionResult> DeleteWishList(long productId)
        {
            var wishListItem = await _dataContext.WishList
                .FirstOrDefaultAsync(w => w.ProductId == productId);

            if (wishListItem == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại trong danh sách yêu thích!";
                return RedirectToAction("WishList");
            }

            _dataContext.WishList.Remove(wishListItem);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Xóa sản phẩm yêu thích thành công!";
            return RedirectToAction("WishList");
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList(long Id)
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