using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NikeStore.Repository.Components
{
    public class ServiceViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ServiceViewComponent(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var service = _context.Service.Include(p => p.ServiceTypes).ToList();
            return View(service);
        }
    }
}
