using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Repository.Components
{
    public class ProductColorViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ProductColorViewComponent(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var productColor = _context.ProductColor.ToList();
            return View(productColor);
        }
    }
}
