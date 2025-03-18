using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Repository.Components
{
    public class ProductSizeViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ProductSizeViewComponent(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var productSize = _context.ProductSize.ToList();
            return View(productSize);
        }
    }
}
