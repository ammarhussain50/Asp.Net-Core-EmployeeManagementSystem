using EMS_Backend.Interface;
using EMS_Backend.Mappers;
using EMS_Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

            //  Check if already marked for today
            var attendenceList = await attendanceRepo.FindAsync(
                x => x.EmployeeId == employeeId.Value &&
                     DateTime.Compare(x.Date.Date, DateTime.UtcNow.Date) == 0
            );

            if (attendenceList != null)
            {
                return BadRequest(new { message = "Already marked present for today." });

            }

            //If not marked, create new record
            var attendence = new Attendance
            {
                Date = DateTime.UtcNow,
                EmployeeId = employeeId.Value,
                Type = (int)AttendanceType.Present
            };
            await attendanceRepo.AddAsync(attendence);
            await attendanceRepo.SaveChangesAsync();





            return Ok(new { message = "Attendance marked as present successfully." });
        }
    }
}
