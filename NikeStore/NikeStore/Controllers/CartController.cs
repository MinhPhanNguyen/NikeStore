using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NikeStore.Models;
using NikeStore.Models.ViewModels;
using NikeStore.Repository;

namespace NikeStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly DataContext _context;

        public CartController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            var shippingPriceCookie = Request.Cookies["ShippingPrice"];
            decimal shippingPrice = 0;

            if(shippingPriceCookie != null)
            {
                var shippingPriceJson = shippingPriceCookie;
                shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
            }

            var coupon_code = Request.Cookies["CouponTitle"];
            decimal coupon_discount = 0; 
            if (Request.Cookies["Discount"] != null)
            {
                decimal.TryParse(Request.Cookies["Discount"], out coupon_discount);
            }

            CartItemViewModel cartItemViewModel = new CartItemViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.TotalPrice),
                ShippingCost = shippingPrice,
                CouponCode = coupon_code,
                Discount = coupon_discount
            };
            return View(cartItemViewModel);
        }

        public async Task<IActionResult> Add(long Id)
        {
            Product product = await _context.Product
                .Include(p => p.ProductColor)
                .Include(p => p.ProductSize)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductID == Id);

            if (product == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.FirstOrDefault(x => x.ProductId == Id);

            if (cartItem == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItem.Quantity++;
            }

            HttpContext.Session.SetJson("Cart", cart);
            TempData["success"] = "Sản phẩm đã được thêm vào giỏ hàng.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(long Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            CartItemModel cartItem = cart.FirstOrDefault(x => x.ProductId == Id);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--; // Giảm số lượng
                }
                else
                {
                    cart.RemoveAll(p => p.ProductId == Id); // Xóa khỏi giỏ hàng nếu số lượng = 1
                }
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart); // Cập nhật lại session
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(long Id)
        {
            Product product = await _context.Product.FirstOrDefaultAsync(p => p.ProductID == Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            CartItemModel cartItem = cart.FirstOrDefault(x => x.ProductId == Id);

            if (cartItem != null && product.Quantity > cartItem.Quantity)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem.Quantity = product.Quantity;   
            }

            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(long Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            cart.RemoveAll(p => p.ProductId == Id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetShipping(Shipping shipping, string quan, string phuong, string tinh)
        {
            var existingShipping = await _context.Shipping
                .FirstOrDefaultAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

            decimal shippingPrice = 0;

            if(existingShipping != null)
            {
                shippingPrice = existingShipping.Price;
            }
            else
            {
                shippingPrice = 30000;
            }

            var shippingPriceJson = JsonConvert.SerializeObject(shippingPrice);
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    Secure = true
                };

                Response.Cookies.Append("ShippingPrice", shippingPriceJson, cookieOptions);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Lỗi thêm giá vận chuyển vào cookies: {ex.Message}");
            }

            return Json(new { shippingPrice });
        }

        [HttpGet]
        public IActionResult DeleteShipping()
        {
            Response.Cookies.Delete("ShippingPrice");
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> GetCoupon(Promotion promotion, string coupon_value)
        {
            var validCoupon = await _context.Promotion.FirstOrDefaultAsync(x => x.Name == coupon_value && x.Quantity >= 1);

            if (validCoupon == null)
            {
                return Ok(new { success = false, message = "Mã giảm giá không hợp lệ hoặc đã hết" });
            }

            string couponTitle = validCoupon.Name + " - " + validCoupon.Description;
            decimal discount = validCoupon.Discount; 
            TimeSpan remainingTime = validCoupon.EndDate - DateTime.Now;
            int dayRemaining = remainingTime.Days;

            if (dayRemaining < 0)
            {
                return Ok(new { success = false, message = "Mã đã hết hạn" });
            }

            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
                Response.Cookies.Append("Discount", discount.ToString(), cookieOptions); 

                return Ok(new
                {
                    success = true,
                    message = "Thêm mã thành công",
                    couponTitle = couponTitle,
                    discount = discount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi thêm mã giảm giá: {ex.Message}");
                return Ok(new { success = false, message = "Thêm mã thất bại" });
            }
        }
    }
}
