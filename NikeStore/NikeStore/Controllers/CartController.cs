using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            CartItemViewModel cartItemViewModel = new CartItemViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.TotalPrice)
            };
            return View(cartItemViewModel);
        }

        public async Task<IActionResult> Add(long Id)
        {
            Product product = await _context.Product
                .Include(p => p.ProductColor)
                .Include(p => p.ProductSize)
                .Include(p => p.Promotion)
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
    }
}
