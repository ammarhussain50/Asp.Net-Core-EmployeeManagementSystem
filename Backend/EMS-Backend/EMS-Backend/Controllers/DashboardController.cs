using EMS_Backend.Entity;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IRepository<Employee> empRepo;
        private readonly IRepository<Department> depRepo;

        public DashboardController(IRepository<Employee> empRepo, IRepository<Department> depRepo)
        {
            this.empRepo = empRepo;
            this.depRepo = depRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TotalSalary()
        {

            var empList = await empRepo.GetAllAsync();
            var depList = await depRepo.GetAllAsync();
            var totalSalary = empList.Sum(x => x.Salary ?? 0);
            var employeeCount = empList.Count;
            var depCount = depList.Count;
            return Ok(new
            {
                TotalSalary = totalSalary,
                EmployeeCount = employeeCount,
                DepartmentCount = depCount
            });

        }
    }
}
