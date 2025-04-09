using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NikeStore.Areas.Admin.ApiController
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Product.Include(p => p.Images).ToListAsync();
            return Ok(products);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Product>> GetProductById(long id)
        {
            var product = await _context.Product.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại" });
            }
            return Ok(product);
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<ActionResult<Product>> AddProductAPI([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new { message = "Dữ liệu sản phẩm không hợp lệ" });
            }

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            if (product.ProductID == 0)
            {
                return StatusCode(500, new { message = "Lỗi: Sản phẩm không được lưu vào database." });
            }

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest(new { message = "ID không khớp" });
            }

            var existingProduct = await _context.Product.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại" });
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.IsHot = product.IsHot;
            existingProduct.IsFavorite = product.IsFavorite;
            existingProduct.CategoryID = product.CategoryID;
            existingProduct.ProductColorID = product.ProductColorID;
            existingProduct.ProductSizeID = product.ProductSizeID;
            existingProduct.ProductTypeID = product.ProductTypeID;
            existingProduct.GenderID = product.GenderID;
            existingProduct.WarehouseID = product.WarehouseID;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Cập nhật sản phẩm thành công", data = existingProduct });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại" });
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa sản phẩm thành công" });
        }
    }
}