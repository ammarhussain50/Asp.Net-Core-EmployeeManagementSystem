using EMS_Backend.Data;
using EMS_Backend.Entity;
using EMS_Backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> employeerepository;

        public EmployeeController(IRepository<Employee> employeerepository)
        {
            this.employeerepository = employeerepository;
        }

        [HttpGet]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> Get()
        {
            return Ok(await employeerepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
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
        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
        {
            // Ensure only DepartmentId is set, not Department navigation property
            model.Department = null;

            await employeerepository.AddAsync(model);
            await employeerepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
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
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await employeerepository.DeleteAsync(id);
            await employeerepository.SaveChangesAsync();
            return Ok();
        }



    }
}
