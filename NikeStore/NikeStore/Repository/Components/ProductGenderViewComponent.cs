using Microsoft.AspNetCore.Mvc;

namespace NikeStore.Repository.Components
{
    public class ProductGenderViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ProductGenderViewComponent(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var productGender = _context.ProductGender.ToList();
            return View(productGender);
        }
    }
}
