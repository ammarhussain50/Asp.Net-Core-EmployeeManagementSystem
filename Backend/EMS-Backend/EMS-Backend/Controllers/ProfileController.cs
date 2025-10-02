using EMS_Backend.Data;
using EMS_Backend.DTO;
using EMS_Backend.Entity;
using EMS_Backend.Interface;
using EMS_Backend.Mappers;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IRepository<Employee> employeeRepo;
       

        public ProfileController(UserManager<AppUser> userManager, IRepository<Employee> employeeRepo )
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
                // 👇 ab poora error bhej do taake frontend pe clear dikhe
                return BadRequest(new { message = "User update failed", errors = result.Errors });

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User does not exist." });

            var employee = await employeeRepo.FindAsync(e => e.AppUserId == id);
            if (employee == null)
                return NotFound(new { message = "Employee does not exist." });


            var profile = (user, employee).ToProfileResponseDto();

            return Ok(profile);
        }

    }
}
