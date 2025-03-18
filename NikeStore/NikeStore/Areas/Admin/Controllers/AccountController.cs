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
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        public AccountController(DataContext context ,UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Account()
        {
            var account = await (from user in _context.Users
                                 join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                 into UserRoles
                                 from userRole in UserRoles.DefaultIfEmpty()
                                 join role in _context.Roles on user.RoleId equals role.Id
                                    into Roles
                                 from role in Roles.DefaultIfEmpty()
                                 select new
                                 {
                                     User = user,
                                     RoleName = role.Name
                                 }).ToListAsync();
            var logginedUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.LogginedUser = logginedUser;

            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(new AppUserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AppUserModel user)
        {
            if (ModelState.IsValid)
            {
                var addUser = await _userManager.CreateAsync(user, user.PasswordHash);
                if (addUser.Succeeded)
                {
                    var createUser = await _userManager.FindByEmailAsync(user.Email);
                    var userId = createUser.Id;
                    var role = _roleManager.FindByIdAsync(user.RoleId);

                    var addToRole = await _userManager.AddToRoleAsync(createUser, role.Result.Name);

                    if(!addToRole.Succeeded)
                    {
                        AddIdentityErrors(addToRole);
                        return View(user);
                    }

                    return RedirectToAction("Account");
                }
                else
                {
                    AddIdentityErrors(addUser);
                    return View(user);  
                }
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

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            TempData["success"] = "Thêm role thành công!";
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, AppUserModel user)
        {
            var existUser = await _userManager.FindByIdAsync(Id);
            if (existUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existUser.UserName = user.UserName;
                existUser.Email = user.Email;
                existUser.RoleId = user.RoleId;
                existUser.PhoneNumber = user.PhoneNumber;

                var updateUser = await _userManager.UpdateAsync(existUser);
                if (updateUser.Succeeded)
                {
                    return RedirectToAction("Account");
                }
                else
                {
                    AddIdentityErrors(updateUser);
                    return View(user);
                }
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

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            TempData["success"] = "Cập nhật role thành công!";
            return View(existUser);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if(string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }
            
            var delete = await _userManager.DeleteAsync(user);
            if (delete.Succeeded)
            {
                return RedirectToAction("Account");
            }
            else
            {
                AddIdentityErrors(delete);
                return View(user);
            }

            TempData["success"] = "Xóa role thành công";
            return RedirectToAction("Account");
        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
