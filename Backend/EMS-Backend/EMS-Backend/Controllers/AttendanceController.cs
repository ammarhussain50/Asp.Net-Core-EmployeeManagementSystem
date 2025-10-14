using EMS_Backend.Interface;
using EMS_Backend.Mappers;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IUserContextService userContext;
        private readonly IRepository<Attendance> attendanceRepo;

        public AttendanceController(IUserContextService userContext , IRepository<Attendance> AttendanceRepo)
        {
            this.userContext = userContext;
            attendanceRepo = AttendanceRepo;
        }

        [HttpPost("mark-present")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MarkAttendance()
        {
            //if (model == null)
            //{
            //    return BadRequest("Invalid leave application data.");
            //}

            // 🔹 Get EmployeeId from JWT claims via UserContextService
            var employeeId = await userContext.GetEmployeeIdFromClaimsAsync(User);
            if (employeeId == null)
                return Unauthorized("Employee not found.");

          
            
            var Attendance = AttendanceMapper.Markattendance(employeeId.Value);
            await attendanceRepo.AddAsync(Attendance);
            await attendanceRepo.SaveChangesAsync();





            return Ok("Attendance marked successfully.");
        }
    }
}
