using EMS_Backend.DTO;
using EMS_Backend.Helpers;
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
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveRepository leaveRepo;
        private readonly IUserContextService userContext;
        private readonly IRepository<Attendance> attendanceRepo;

        public LeaveController(ILeaveRepository LeaveRepo, IUserContextService UserContext , IRepository<Attendance> AttendanceRepo )
        {
            leaveRepo = LeaveRepo;
            userContext = UserContext;
            attendanceRepo = AttendanceRepo;
        }

        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid leave application data.");
            }

            // 🔹 Get EmployeeId from JWT claims via UserContextService
            var employeeId = await userContext.GetEmployeeIdFromClaimsAsync(User);
            if (employeeId == null)
                return Unauthorized("Employee not found.");


            // Use the mapper to create a Leave entity
            var leave = model.ToLeave((int)employeeId);

            await leaveRepo.AddAsync(leave);
            await leaveRepo.SaveChangesAsync();
            return Ok(new { Message = "Leave application submitted successfully." });
        }

        [HttpPut("update-leave")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid leave update data.");
            }
            var leave = await leaveRepo.GetByIdAsync(model.Id!.Value);
            if (leave == null)
            {
                return NotFound("Leave application not found.");
            }
            //  Check role from JWT claim
            var isAdmin = userContext.IsAdmin(User);

            if (isAdmin)
            {
                //  Admin can update any status (use mapper directly)
                leave.UpdateLeaveFromDto(model);

                if (model.Status == (int)LeaveStatus.Accepted)
                {
                    //  If leave is accepted, we mark the leave on attendence as well
                    //If not marked, create new record
                    var attendence = new Attendance
                    {
                        Date = leave.LeaveDate, //isliye kia q k leavedate DateOnly h jo mene datetime k bajae rkhi th is liye type mismatch sy bachny k liye convert kia
                        EmployeeId = leave.EmployeeId,
                        Type = (int)AttendanceType.Absent,
                    };
                    await attendanceRepo.AddAsync(attendence);

                }
            }
            else
            {
                //  Employee can only cancel their leave
                if (model.Status == (int)LeaveStatus.Cancelled)
                {
                    leave.UpdateLeaveFromDto(model);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only admin can change this status." });

                }
            }

            leaveRepo.Update(leave);
            await leaveRepo.SaveChangesAsync();

            return Ok(new { message = "Leave updated successfully" });
        }



        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> LeavesList([FromQuery] SearchOptions options)
        {
            //  Check role from JWT claim
            var isAdmin = userContext.IsAdmin(User);

            if (isAdmin)
            {
                var result = await leaveRepo.GetAllAsync(options);
                return Ok(result);
            }
            else
            {
                var employeeId = await userContext.GetEmployeeIdFromClaimsAsync(User);
                if (employeeId == null)
                    return Unauthorized("Employee not found.");

                var result = await leaveRepo.GetByEmployeeIdAsync(employeeId.Value, options);
                return Ok(result);

            }




        }


    }
}
