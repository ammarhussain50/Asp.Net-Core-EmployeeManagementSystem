using EMS_Backend.Data;
using EMS_Backend.Entity;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> employeerepository;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public EmployeeController(IRepository<Employee> employeerepository, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.employeerepository = employeerepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return Ok(await employeerepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var employee = await employeerepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
        {
            // 1. Create Identity user
            var user = new AppUser
            {
                UserName = model.Email,   // ya model.Name agar username alag lena ho
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, "Employee@123"); // fixed password
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // 2. Ensure Employee role exists
            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            // 3. Assign Employee role
            await userManager.AddToRoleAsync(user, "Employee");

            // 4. Save employee in custom Employee table with UserId
            model.AppUserId = user.Id;   //  Identity user ka Id store
            // Ensure only DepartmentId is set, not Department navigation property
            //model.Department = null;

            await employeerepository.AddAsync(model);
            await employeerepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee model)
        {
            var employee = await employeerepository.GetByIdAsync(id);
            employee.Name = model.Name;
            employee.Email = model.Email;
            employee.Phone = model.Phone;
            employee.DepartmentId = model.DepartmentId;
            employee.LastWorkingDate = model.LastWorkingDate;
            employee.JobTitle = model.JobTitle;

            employeerepository.Update(employee);
            await employeerepository.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await employeerepository.DeleteAsync(id);
            await employeerepository.SaveChangesAsync();
            return Ok();
        }



    }
}
