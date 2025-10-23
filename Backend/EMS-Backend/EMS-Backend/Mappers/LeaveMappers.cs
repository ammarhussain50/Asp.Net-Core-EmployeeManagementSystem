using EMS_Backend.DTO;
using EMS_Backend.Model;

namespace EMS_Backend.Mappers
{
    public static class LeaveMappers
    {
        public static Leave ToLeave(this LeaveDto dto,int empid)
        {
            var date = TimeZoneInfo.ConvertTimeFromUtc(dto.LeaveDate.Value, TimeZoneInfo.Local);
            return new Leave
            {
                Type = dto.Type ?? (int)LeaveType.Sick, // Default value if null
                Reason = dto.Reason!,
                LeaveDate = date,
                Status = (int)LeaveStatus.Pending,
                EmployeeId = empid
            };
        }
        public static void UpdateLeaveFromDto(this Leave leave, LeaveDto dto)
        {
            leave.Status = dto.Status!.Value;


        }
    }
}
