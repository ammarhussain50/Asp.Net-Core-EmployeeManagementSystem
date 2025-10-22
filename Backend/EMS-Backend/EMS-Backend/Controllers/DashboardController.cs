using EMS_Backend.Data;
using EMS_Backend.DTO;
using EMS_Backend.Entity;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IRepository<Employee> empRepo;
        private readonly IRepository<Department> depRepo;
        private readonly AppDbContext context;

        public DashboardController(IRepository<Employee> empRepo, IRepository<Department> depRepo, AppDbContext context)
        {
            this.empRepo = empRepo;
            this.depRepo = depRepo;
            this.context = context;
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

        [HttpGet("department-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDepartmentData()
        {
            // Only showing departments that currently have employees
            //yeh hm db level pr kr rhy hn taake zyada efficient ho
            // yeh stored procedure nhi hy complex project my stored procedure use kr skty hen
            var result = await context.Employees
                .GroupBy(e => e.DepartmentId)
                .Select(g => new DepartmentDataDto
                {
                    Name = g.First().Department.Name,
                    EmployeeCount = g.Count()
                })
                .OrderByDescending(x => x.EmployeeCount)
                .ToListAsync();

            return Ok(result);
        }

    }
}
