using EMS_Backend.DTO;
using EMS_Backend.Entity;
using EMS_Backend.Interface;
using EMS_Backend.Mappers;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IRepository<Employee> employeeRepo;

        public ProfileController(UserManager<AppUser> userManager, IRepository<Employee> employeeRepo)
        {
            this.userManager = userManager;
            this.employeeRepo = employeeRepo;
        }

        [Authorize]
        [HttpPost("Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto model)
        {
            // Get current logged-in user
            var userId = model.UserId;

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User does not exist.");

            // 🔹 Update User (AspNetUsers) - except password
            user.UpdateUserFromDto(model);
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 🔹 Update Employee 
            var employee = await employeeRepo.FindAsync(e => e.AppUserId == userId);
            if (employee != null)
            {
                employee.UpdateEmployeeFromDto(model);
                employeeRepo.Update(employee);
                await employeeRepo.SaveChangesAsync();
            }

            // 🔹 Update Password (if provided)
            if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var pwdResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!pwdResult.Succeeded)
                    return BadRequest(pwdResult.Errors);
            }

            return Ok("Profile updated successfully.");
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {

            // 1. JWT se current logged-in user ka id nikaal lo
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token.");

            // 1. Find User
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            // 2. Find Employee
            var employee = await employeeRepo.FindAsync(e => e.AppUserId == userId);

            // 3. Map to DTO
            var dto = new ProfileDto
            {
                //UserId = user.Id,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Name = employee?.Name,   // null-check in case employee record missing
                Phone = employee?.Phone
            };

            return Ok(dto);
        }

    }
}
