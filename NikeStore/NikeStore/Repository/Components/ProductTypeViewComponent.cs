using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Repository.Components
{
    public class ProductTypeViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ProductTypeViewComponent(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var productType = _context.ProductType.ToList();
            return View(productType);
        }
    }
}
