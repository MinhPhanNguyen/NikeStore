using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;
using System.IO;
using System.Threading.Tasks;

namespace NikeStore.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ServiceApiController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServiceApiController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Service.Include(p => p.ServiceTypes).ToListAsync();
            return Ok(services);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetServiceTypes()
        {
            var serviceTypes = await _context.ServiceType.Include(p => p.Service).ToListAsync();
            return Ok(serviceTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Service.FindAsync(id);
            if (service == null) return NotFound();
            return Ok(service);
        }

        [HttpGet("types/{id}")]
        public async Task<IActionResult> GetServiceTypesById(int id)
        {
            var serviceTypes = await _context.ServiceType.FindAsync(id);
            if (serviceTypes == null) return NotFound();
            return Ok(serviceTypes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] Service service)
        {
            var existingService = await _context.Service.FindAsync(id);
            if (existingService == null) return NotFound();

            existingService.Name = service.Name;
            existingService.Description = service.Description;

            if (service.ImageUpload != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/services");
                string imageName = Path.GetRandomFileName() + Path.GetExtension(service.ImageUpload.FileName);
                string filePath = Path.Combine(uploadDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await service.ImageUpload.CopyToAsync(fs);
                }
                existingService.ImageUrl = imageName;
            }

            _context.Service.Update(existingService);
            await _context.SaveChangesAsync();
            return Ok(existingService);
        }

        [HttpPost("types")]
        public async Task<IActionResult> AddServiceType([FromBody] ServiceType serviceType)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.ServiceType.Add(serviceType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServiceTypes), new { id = serviceType.ServiceTypeID }, serviceType);
        }

        [HttpPut("types/{id}")]
        public async Task<IActionResult> UpdateServiceType(int id, [FromBody] ServiceType serviceType)
        {
            var existingServiceType = await _context.ServiceType.FindAsync(id);
            if (existingServiceType == null) return NotFound();

            existingServiceType.Name = serviceType.Name;
            existingServiceType.Description = serviceType.Description;

            _context.ServiceType.Update(existingServiceType);
            await _context.SaveChangesAsync();
            return Ok(existingServiceType);
        }

        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteServiceType(int id)
        {
            var serviceType = await _context.ServiceType.FindAsync(id);
            if (serviceType == null) return NotFound();

            _context.ServiceType.Remove(serviceType);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa dữ liệu vận chuyển thành công" });
        }
    }
}
