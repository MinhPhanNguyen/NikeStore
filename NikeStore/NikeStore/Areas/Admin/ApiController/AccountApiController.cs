using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NikeStore.Areas.Admin.ApiController
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public AccountApiController(DataContext context, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAccounts()
        {
            var accounts = await (from user in _context.Users
                                  join userRole in _context.UserRoles on user.Id equals userRole.UserId into UserRoles
                                  from userRole in UserRoles.DefaultIfEmpty()
                                  join role in _context.Roles on user.RoleId equals role.Id into Roles
                                  from role in Roles.DefaultIfEmpty()
                                  select new
                                  {
                                      User = user,
                                      RoleName = role.Name
                                  }).ToListAsync();

            var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(new { loggedInUser, accounts });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] AppUserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUser = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!addUser.Succeeded)
            {
                return BadRequest(new { errors = addUser.Errors });
            }

            var createdUser = await _userManager.FindByEmailAsync(user.Email);
            if (createdUser == null)
            {
                return StatusCode(500, "Không thể tìm thấy tài khoản vừa tạo.");
            }

            var role = await _roleManager.FindByIdAsync(user.RoleId);
            if (role == null)
            {
                return BadRequest(new { message = "Vai trò không hợp lệ" });
            }

            var addToRole = await _userManager.AddToRoleAsync(createdUser, role.Name);
            if (!addToRole.Succeeded)
            {
                return BadRequest(new { errors = addToRole.Errors });
            }

            return CreatedAtAction(nameof(GetAccounts), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("role/{roleName}")]
        public async Task<ActionResult<IEnumerable<AppUserModel>>> GetUserByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound(new { message = "Vai trò không tồn tại" });
            }

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return Ok(users);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<AppUserModel>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }
            return Ok(user);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] AppUserModel user)
        {
            var existUser = await _userManager.FindByIdAsync(id);
            if (existUser == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existUser.UserName = user.UserName;
            existUser.Email = user.Email;
            existUser.RoleId = user.RoleId;
            existUser.PhoneNumber = user.PhoneNumber;

            var updateUser = await _userManager.UpdateAsync(existUser);
            if (!updateUser.Succeeded)
            {
                return BadRequest(new { errors = updateUser.Errors });
            }

            return Ok(new { message = "Cập nhật thành công", data = existUser });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            var delete = await _userManager.DeleteAsync(user);
            if (!delete.Succeeded)
            {
                return BadRequest(new { errors = delete.Errors });
            }

            return Ok(new { message = "Xóa tài khoản thành công" });
        }
    }
}
