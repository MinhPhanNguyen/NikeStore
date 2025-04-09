using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;

namespace NikeStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(string Slug = "", string sort_by = "", decimal? startprice = null, decimal? endprice = null)
        {
            if (string.IsNullOrWhiteSpace(Slug))
            {
                return RedirectToAction("Index", "Home");
            }

            var category = await _dataContext.ProductCategory
                .FirstOrDefaultAsync(c => c.Slug == Slug);

            if (category == null)
            {
                return RedirectToAction("Index", "Home");
            }

            IQueryable<Product> productsQuery = _dataContext.Product
                .Include(p => p.Images)
                .Where(p => p.CategoryID == category.CategoryID);

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

            var productsByCategory = await productsQuery.ToListAsync();

            return View(productsByCategory);
        }
    }
}
