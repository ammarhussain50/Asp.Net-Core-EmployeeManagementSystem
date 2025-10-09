using EMS_Backend.DTO;
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
        private readonly IRepository<Leave> leaveRepo;
        private readonly IUserContextService userContext;

        public LeaveController(IRepository<Leave> LeaveRepo, IUserContextService UserContext)
        {
            leaveRepo = LeaveRepo;
            userContext = UserContext;
        }

        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
        {
            if(model == null)
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

        //[HttpPost("apply")]
        //[Authorize(Roles = "Employee,Admin")]
        //public async Task<IActionResult> LeaveStatus([FromBody] LeaveDto model)
        //{
        //    var leave = await leaveRepo.GetByIdAsync(model.Id!.Value);
        //    leave.Status = model.Status!.Value;
        //    //leaveRepo.Update(leave);
        //    await leaveRepo.SaveChangesAsync();
        //    return Ok();
        //}
    }
}
