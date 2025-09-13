using EMS_Backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext dbcontext;

        public EmployeeController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(dbcontext.Employees.ToList());
        }
    }
}
