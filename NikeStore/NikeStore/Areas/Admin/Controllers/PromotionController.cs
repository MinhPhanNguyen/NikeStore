using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Repository;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PromotionController : Controller
    {
        private readonly DataContext _context;
        public PromotionController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Promotion()
        {
            var promotion = await _context.Promotion.ToListAsync();
            return View(promotion);
        }
    }
}
