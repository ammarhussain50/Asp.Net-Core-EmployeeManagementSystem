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

        public LeaveController(IRepository<Leave> LeaveRepo)
        {
            leaveRepo = LeaveRepo;
        }

        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
        {
            if(model == null)
            {
                return BadRequest("Invalid leave application data.");
            }

            // Use the mapper to create a Leave entity
            var leave = model.ToLeave();

            await leaveRepo.AddAsync(leave);
            await leaveRepo.SaveChangesAsync();
            return Ok(new { Message = "Leave application submitted successfully." });
        }
    }
}
