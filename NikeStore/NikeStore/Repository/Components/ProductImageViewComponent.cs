using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Repository.Components
{
    public class ProductImageViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ProductImageViewComponent(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var productImage = _context.ProductImage.ToList();
            return View(productImage);
        }
    }
}
