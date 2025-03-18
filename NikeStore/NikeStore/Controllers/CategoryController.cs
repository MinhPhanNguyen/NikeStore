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

        public async Task<IActionResult> Index(string Slug = "")
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

            // Tìm sản phẩm theo danh mục
            var productsByCategory = await _dataContext.Product
                .Include(p => p.Images)
                .Where(p => p.CategoryID == category.CategoryID)
                .OrderByDescending(p => p.ProductID)
                .ToListAsync();

            return View(productsByCategory);
        }
    }
}
