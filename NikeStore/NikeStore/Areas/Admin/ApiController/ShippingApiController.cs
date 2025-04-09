using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NikeStore.Areas.Admin.ApiController
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ShippingApiController : ControllerBase
    {
        private readonly DataContext _context;

        public ShippingApiController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetShippings()
        {
            var shippings = await _context.Shipping.ToListAsync();
            return Ok(shippings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shipping>> GetShippingById(int id)
        {
            var shipping = await _context.Shipping.FindAsync(id);
            if (shipping == null)
            {
                return NotFound(new { message = "Dữ liệu vận chuyển không tồn tại" });
            }
            return Ok(shipping);
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<ActionResult<Shipping>> AddShippingAPI([FromBody] Shipping shipping)
        {
            if (shipping == null)
            {
                return BadRequest(new { message = "Dữ liệu vận chuyển không hợp lệ" });
            }

            var existingShipping = await _context.Shipping.AnyAsync(x =>
                x.City == shipping.City &&
                x.District == shipping.District &&
                x.Ward == shipping.Ward
            );

            if (existingShipping)
            {
                return Conflict(new { message = "Dữ liệu vận chuyển đã tồn tại" });
            }

            _context.Shipping.Add(shipping);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShippingById), new { id = shipping.Id }, shipping);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipping(int id, [FromBody] Shipping shipping)
        {
            if (id != shipping.Id)
            {
                return BadRequest(new { message = "ID không khớp" });
            }

            var existingShipping = await _context.Shipping.FindAsync(id);
            if (existingShipping == null)
            {
                return NotFound(new { message = "Dữ liệu vận chuyển không tồn tại" });
            }

            existingShipping.City = shipping.City;
            existingShipping.District = shipping.District;
            existingShipping.Ward = shipping.Ward;
            existingShipping.Price = shipping.Price;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Cập nhật thông tin vận chuyển thành công", data = existingShipping });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipping(int id)
        {
            var shipping = await _context.Shipping.FindAsync(id);
            if (shipping == null)
            {
                return NotFound(new { message = "Dữ liệu vận chuyển không tồn tại" });
            }

            _context.Shipping.Remove(shipping);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa dữ liệu vận chuyển thành công" });
        }
    }
}
