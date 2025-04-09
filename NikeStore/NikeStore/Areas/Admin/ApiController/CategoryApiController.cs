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
    public class CategoryApiController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetCategories()
        {
            var categories = await _context.ProductCategory.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetCategoryById(int id)
        {
            var category = await _context.ProductCategory.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Danh mục không tồn tại" });
            }
            return Ok(category);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ProductCategory>> AddCategoryAPI([FromBody] ProductCategory category)
        {
            if (category == null)
            {
                return BadRequest(new { message = "Dữ liệu danh mục không hợp lệ" });
            }

            _context.ProductCategory.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryID }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ProductCategory category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest(new { message = "ID không khớp" });
            }

            var existingCategory = await _context.ProductCategory.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Danh mục không tồn tại" });
            }

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Cập nhật danh mục thành công", data = existingCategory });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.ProductCategory.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Danh mục không tồn tại" });
            }

            _context.ProductCategory.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa danh mục thành công" });
        }
    }
}
