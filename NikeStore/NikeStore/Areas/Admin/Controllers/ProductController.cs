using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NikeStore.Models;
using NikeStore.Repository;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Product()
        {
            List<Product> product = await _context.Product.OrderBy(p => p.ProductID)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductGender)
                .Include(p => p.ProductSize)
                .Include(p => p.ProductColor)
                .Include(p => p.ProductType)
                .Include(p => p.Images)
                .ToListAsync();
           

            return View(product);
        }

        public async Task<IActionResult> HotProduct()
        {
            List<Product> product = await _context.Product.OrderBy(p => p.ProductID)
               .Include(p => p.ProductCategory)
               .Include(p => p.ProductGender)
               .Include(p => p.ProductSize)
               .Include(p => p.ProductColor)
               .Include(p => p.ProductType)
               .Include(p => p.Images)
               .Where(p => p.IsHot == true)
               .ToListAsync();

            return View(product);
        }

        public async Task<IActionResult> LoveProduct()
        {
            List<Product> product = await _context.Product.OrderBy(p => p.ProductID)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductGender)
                .Include(p => p.ProductSize)
                .Include(p => p.ProductColor)
                .Include(p => p.ProductType)
                .Include(p => p.Images)
                .Where(p => p.IsFavorite == true)
                .ToListAsync();

            return View(product);
        }

        //public async Task<IActionResult> ReviewProduct()
        //{
        //    var productReview = await _context.ProductReview.OrderBy(p => p.ReviewId)
        //        .Include(p => p.Product)
        //        .ToListAsync();
        //    return View(productReview);                                                                                                                                                                                 
        //}

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory, "CategoryID", "CategoryName");
            ViewBag.ProductType = new SelectList(_context.ProductType, "ProductTypeID", "Type");
            ViewBag.ProductColor = new SelectList(_context.ProductColor, "ProductColorID", "Color");
            ViewBag.ProductGender = new SelectList(_context.ProductGender, "GenderID", "GenderName");
            ViewBag.ProductSize = new SelectList(_context.ProductSize, "ProductSizeID", "Size");
            ViewBag.Warehouse = new SelectList(_context.Warehouse, "WarehouseID", "WarehouseName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product)
        {
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProductType = new SelectList(_context.ProductType, "ProductTypeID", "Type", product.ProductTypeID);
            ViewBag.ProductColor = new SelectList(_context.ProductColor, "ProductColorID", "Color", product.ProductColorID);
            ViewBag.ProductGender = new SelectList(_context.ProductGender, "GenderID", "GenderName", product.GenderID);
            ViewBag.ProductSize = new SelectList(_context.ProductSize, "ProductSizeID", "Size", product.ProductSizeID);
            ViewBag.Warehouse = new SelectList(_context.Warehouse, "WarehouseID", "WarehouseName", product.WarehouseID);

            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Product.FirstOrDefaultAsync(p => p.Name == product.Name);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong Database");
                    return View(product);
                }

                var category = await _context.ProductCategory.FindAsync(product.CategoryID);
                if (category != null)
                {
                    product.Slug = GenerateSlug(category.CategoryName);
                }
                else
                {
                    ModelState.AddModelError("", "Danh mục không tồn tại.");
                    return View(product);
                }


                _context.Product.Add(product);
                await _context.SaveChangesAsync();

                if (product.ImageUploads != null && product.ImageUploads.Count > 0)
                {
                    product.Images = new List<ProductImage>();

                    string productDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products", product.ProductID.ToString());
                    if (!Directory.Exists(productDir))
                    {
                        Directory.CreateDirectory(productDir);
                    }

                    foreach (var imageFile in product.ImageUploads)
                    {
                        string imgName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(productDir, imgName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fs);
                        }

                        product.Images.Add(new ProductImage
                        {
                            ImageUrl = $"/media/products/{product.ProductID}/{imgName}",
                            ProductID = product.ProductID
                        });
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Product");
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

        public async Task<IActionResult> Edit(long Id)
        {
            Product product = await _context.Product.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductID == Id);
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProductType = new SelectList(_context.ProductType, "ProductTypeID", "Type", product.ProductTypeID);
            ViewBag.ProductColor = new SelectList(_context.ProductColor, "ProductColorID", "Color", product.ProductColorID);
            ViewBag.ProductGender = new SelectList(_context.ProductGender, "GenderID", "GenderName", product.GenderID);
            ViewBag.ProductSize = new SelectList(_context.ProductSize, "ProductSizeID", "Size", product.ProductSizeID);
            ViewBag.Warehouse = new SelectList(_context.Warehouse, "WarehouseID", "WarehouseName", product.WarehouseID);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long Id, Product product)
        {
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ProductType = new SelectList(_context.ProductType, "ProductTypeID", "Type", product.ProductTypeID);
            ViewBag.ProductColor = new SelectList(_context.ProductColor, "ProductColorID", "Color", product.ProductColorID);
            ViewBag.ProductGender = new SelectList(_context.ProductGender, "GenderID", "GenderName", product.GenderID);
            ViewBag.ProductSize = new SelectList(_context.ProductSize, "ProductSizeID", "Size", product.ProductSizeID);
            ViewBag.Warehouse = new SelectList(_context.Warehouse, "WarehouseID", "WarehouseName", product.WarehouseID);

            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Product.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductID == Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryID = product.CategoryID;
                existingProduct.ProductTypeID = product.ProductTypeID;
                existingProduct.ProductColorID = product.ProductColorID;
                existingProduct.GenderID = product.GenderID;
                existingProduct.ProductSizeID = product.ProductSizeID;

                var category = await _context.ProductCategory.FindAsync(product.CategoryID);
                if (category != null)
                {
                    existingProduct.Slug = GenerateSlug(category.CategoryName);
                }
                else
                {
                    ModelState.AddModelError("", "Danh mục không tồn tại.");
                    return View(product);
                }

                if (product.ImageUploads != null && product.ImageUploads.Count > 0)
                {
                    string productDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products", existingProduct.ProductID.ToString());
                    if (!Directory.Exists(productDir))
                    {
                        Directory.CreateDirectory(productDir);
                    }

                    foreach (var imageFile in product.ImageUploads)
                    {
                        string imageName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(productDir, imageName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fs);
                        }
                        
                        var image = new ProductImage
                        {
                            ImageUrl = $"/media/products/{existingProduct.ProductID}/{imageName}",
                            ProductID = existingProduct.ProductID
                        };
                        _context.ProductImage.Add(image);
                    }
                }

                _context.Update(existingProduct);
                await _context.SaveChangesAsync();

                TempData["success"] = "Update product successfully";
                return RedirectToAction("Product");
            }

            TempData["error"] = "Model bị lỗi, vui lòng thử lại.";
            return View(product);
        }

        private string GenerateSlug(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";

            string slug = name.ToLower().Trim();
            slug = Regex.Replace(slug, @"\s+", "");
            slug = Regex.Replace(slug, @"[^a-z0-9]", "");

            return slug;
        }

        public async Task<IActionResult> Delete(long Id)
        {
            var product = await _context.Product
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductID == Id);

            if (product == null)
            {
                return NotFound();
            }

            string productDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products", product.ProductID.ToString());

            if (Directory.Exists(productDir))
            {
                Directory.Delete(productDir, true); 
            }

            _context.ProductImage.RemoveRange(product.Images);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Product");
        }

        [HttpGet]
        public async Task<IActionResult> AddQuantity(long Id)
        {
            var productByQuantity = await _context.ProductQuantity.Where(p => p.ProductId == Id).ToListAsync();
            ViewBag.ProductQuantity = productByQuantity;
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StoreProductQuantity(ProductQuantity productQuantity, long ProductId)
        {
            Console.WriteLine($"ProductId nhận được: {ProductId}");
            var product = await _context.Product.FindAsync(ProductId);

            if (product == null)
            {
                return NotFound();
            }

            product.Quantity += productQuantity.Quantity;
            productQuantity.ProductId = ProductId;
            productQuantity.CreatedAt = DateTime.Now;

            _context.Add(productQuantity);
            await _context.SaveChangesAsync();

            TempData["success"] = "Successfully";
            return RedirectToAction("AddQuantity", new { Id = ProductId });
        }
    }
}
