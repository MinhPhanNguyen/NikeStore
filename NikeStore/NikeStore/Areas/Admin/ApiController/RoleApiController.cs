using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NikeStore.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class RoleApiController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleApiController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.OrderByDescending(r => r.Id).ToListAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }
            return Ok(role);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole([FromBody] IdentityRole model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest(new { message = "Role name is required" });
            }

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                return BadRequest(new { message = "Role already exists" });
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Add new role successfully" });
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] IdentityRole model)
        {
            if (string.IsNullOrEmpty(id) || model == null)
            {
                return BadRequest(new { message = "Invalid role data" });
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Update role successfully" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "Invalid role ID" });
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Delete role successfully" });
        }
    }
}
