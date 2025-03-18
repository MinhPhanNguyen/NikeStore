using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Category()
        {
            List<ProductCategory> category = await _context.ProductCategory.OrderBy(p => p.CategoryID)
                .ToListAsync();

            return View(category);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductCategory product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _context.ProductCategory.FirstOrDefaultAsync(p => p.CategoryName == product.CategoryName);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong Database");
                    return View(product);
                }

                product.Slug = GenerateSlug(product.CategoryName);

                if (product.ImageUploads != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUploads.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUploads.CopyToAsync(fs);
                    fs.Close();
                    product.ImageUrl = imageName;
                }

                _context.ProductCategory.Add(product);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Category");
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

            return View(product);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            ProductCategory product = await _context.ProductCategory.FindAsync(Id);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductCategory product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _context.ProductCategory.FindAsync(Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.CategoryName = product.CategoryName;
                existingProduct.Description = product.Description;

                product.Slug = GenerateSlug(product.CategoryName);

                if (product.ImageUploads != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUploads.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUploads.CopyToAsync(fs);
                    }
                    existingProduct.ImageUrl = imageName;
                }
                else
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }

                _context.Update(existingProduct);
                await _context.SaveChangesAsync();
                TempData["success"] = "Update product successfully";
                return RedirectToAction("Category");
            }

            TempData["error"] = "Model bị lỗi, vui lòng thử lại.";
            return View(product);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var product = await _context.ProductCategory
                .FirstOrDefaultAsync(p => p.CategoryID == Id);

            if (product == null)
            {
                return NotFound();
            }

            string productDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");

            if (Directory.Exists(productDir))
            {
                Directory.Delete(productDir, true);
            }

            _context.ProductCategory.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Category");
        }

        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = input.ToLowerInvariant();
            input = RemoveDiacritics(input);
            input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
            input = Regex.Replace(input, @"\s+", "-").Trim('-');

            return input;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
