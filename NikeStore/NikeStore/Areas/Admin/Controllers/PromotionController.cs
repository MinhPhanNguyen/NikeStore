using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Promotion promotion)
        {
            if(ModelState.IsValid)
            {
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                TempData["success"] = "Successfully";
                return RedirectToAction("Promotion", "Promotion");
            }
            else
            {
                TempData["error"] = "Model bị lỗi vui lòng thử lại";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(promotion);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Promotion promotion = await _context.Promotion.FirstOrDefaultAsync(p => p.Id == Id);
            return View(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Promotion promotion, int Id)
        {
            if (ModelState.IsValid)
            {
                var existPromotion = await _context.Promotion.FirstOrDefaultAsync(p => p.Id == Id);

                if (existPromotion == null) {
                    return NotFound();
                }

                existPromotion.Name = promotion.Name;
                existPromotion.Description = promotion.Description;
                existPromotion.Discount = promotion.Discount;
                existPromotion.StartDate = promotion.StartDate;
                existPromotion.EndDate = promotion.EndDate;
                existPromotion.Quantity = promotion.Quantity;
                existPromotion.Status = promotion.Status;

                _context.Update(existPromotion);
                await _context.SaveChangesAsync();
                TempData["success"] = "Successfully";
                return RedirectToAction("Promotion");
            }
            else
            {
                TempData["error"] = "Model bị lỗi vui lòng thử lại";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(promotion);
        }
    }
}
