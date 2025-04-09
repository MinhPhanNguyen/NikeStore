using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NikeStore.Models;
using NikeStore.Models.ViewModels;
using NikeStore.Repository;

namespace NikeStore.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly DataContext _context;

        public CheckOutController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CheckOut()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var shippingPriceCookie = Request.Cookies["ShippingPrice"];
                decimal shippingPrice = 0;
                var couponCode = Request.Cookies["CouponTitle"];
                decimal coupon_discount = 0;
                if (Request.Cookies["Discount"] != null)
                {
                    decimal.TryParse(Request.Cookies["Discount"], out coupon_discount);
                }

                if (shippingPriceCookie != null)
                {
                    var shippingPriceJson = shippingPriceCookie;
                    shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
                }

                var orderCode = Guid.NewGuid().ToString();
                var orderItem = new Order
                {
                    OrderCode = orderCode,
                    ShippingCost = shippingPrice,
                    CouponCode = couponCode,
                    Discount = coupon_discount,
                    Status = 1,
                    UserName = userEmail,
                    CreateDate = DateTime.Now
                };

                _context.Order.Add(orderItem);

                List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                var orderDetails = new List<OrderDetail>();

                foreach (var item in cart)
                {
                    orderDetails.Add(new OrderDetail
                    {
                        OrderCode = orderCode,
                        UserName = userEmail,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });

                    var product = await _context.Product.FirstAsync(p => p.ProductID == item.ProductId);
                    product.Quantity -= item.Quantity;
                    product.Sold += item.Quantity;
                    _context.Update(product);
                }

                _context.OrderDetail.AddRange(orderDetails);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Cart");

                var receiver = userEmail;
                var subject = "Đặt hàng thành công ";
                var message = "Đặt hàng thành công. Vui lòng chờ để được duyệt đơn hàng, đây là mã code đơn hàng của bạn: " + orderCode;

                TempData["success"] = "Order has been created successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while processing your order.";
                return RedirectToAction("Index", "Cart");
            }

            return RedirectToAction("Index", "Cart");
        }

    }
}
