using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Repository.Components
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ProductCategoryViewComponent(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var productCategory = _context.ProductCategory.ToList();
            return View(productCategory);
        }
    }
}
