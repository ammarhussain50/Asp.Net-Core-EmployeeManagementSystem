using EMS_Backend.Helpers;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using EMS_Backend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;

        //private readonly IRepository<Department> departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
            //this.departmentRepository = departmentRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> addDepartment([FromBody] Department model)
        {
            await departmentRepository.AddAsync(model);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> updateDepartment(int id, [FromBody] Department model)
        {
            var department = await departmentRepository.GetByIdAsync(id);
            department.Name = model.Name;
            departmentRepository.Update(department);

            await departmentRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getAllDepartment([FromQuery] SearchOptions options)
        {
            var result = await departmentRepository.GetAllAsync(options);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await departmentRepository.DeleteAsync(id);
            await departmentRepository.SaveChangesAsync();

            return Ok();

        }
    }
}