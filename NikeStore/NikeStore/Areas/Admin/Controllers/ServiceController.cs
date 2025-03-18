using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace NikeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServiceController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Service()
        {
           var service = await _context.Service.Include(p => p.ServiceTypes).ToListAsync();
            return View(service);
        }

        public async Task<IActionResult> ServiceType()
        {
            var service = await _context.ServiceType.Include(p => p.Service).OrderBy(p => p.ServiceID).ToListAsync();
            return View(service);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var service = await _context.Service.FindAsync(Id);
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, Service service)
        {
            if (ModelState.IsValid)
            {
                var existingService = await _context.Service.FindAsync(Id);

                if (existingService == null)
                {
                    return NotFound();
                }

                existingService.Name = service.Name;
                existingService.Description = service.Description;


                if (service.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/services");
                    string imageName = Guid.NewGuid().ToString() + "_" + service.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await service.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existingService.ImageUrl = imageName;
                }

                _context.Service.Update(existingService);
                await _context.SaveChangesAsync();
                TempData["success"] = "Service has been updated successfully";
                return RedirectToAction("Service");
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

            return View(service);
        }

        [HttpGet]
        public IActionResult AddType()
        {
            ViewBag.Service = new SelectList(_context.Service, "ServiceID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddType(ServiceType serviceType)
        {
            ViewBag.Service = new SelectList(_context.Service, "ServiceID", "Name", serviceType.ServiceID);
            if (ModelState.IsValid)
            {
                _context.ServiceType.Add(serviceType);
                await _context.SaveChangesAsync();
                TempData["success"] = "Service type has been added successfully";
                return RedirectToAction("ServiceType");
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

            return View(serviceType);
        }

        [HttpGet]
        public async Task<IActionResult> EditType(int Id)
        {
            ServiceType serviceType = await _context.ServiceType.FindAsync(Id);
            ViewBag.Service = new SelectList(_context.Service, "ServiceID", "Name", serviceType.ServiceID);
            return View(serviceType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditType(int Id, ServiceType serviceType)
        {
            ViewBag.Service = new SelectList(_context.Service, "ServiceID", "Name", serviceType.ServiceID);
            if (ModelState.IsValid)
            {
                var existingService = await _context.ServiceType.FindAsync(Id);

                if (existingService == null)
                {
                    return NotFound();
                }

                existingService.Name = serviceType.Name;
                existingService.Description = serviceType.Description;

                _context.ServiceType.Update(existingService);
                await _context.SaveChangesAsync();
                TempData["success"] = "Service type has been updated successfully";
                return RedirectToAction("ServiceType");
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

            return View(serviceType);
        }

        public async Task<IActionResult> DeleteType(int Id)
        {
            var serviceType = await _context.ServiceType.FindAsync(Id);

            _context.ServiceType.Remove(serviceType);
            await _context.SaveChangesAsync();
            TempData["success"] = "Delete service type successfully";
            return RedirectToAction("ServiceType");
        }
    }
}
