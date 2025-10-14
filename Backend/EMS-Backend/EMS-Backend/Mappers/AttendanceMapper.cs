using EMS_Backend.Model;

namespace EMS_Backend.Mappers
{
    public static class AttendanceMapper
    {

        public static Attendance Markattendance(int employeeId)
        {
            return new Attendance
            {
                EmployeeId = employeeId,
                Date = DateTime.Now,
                Type = (int)AttendanceType.Present
            };
        }

    }
}
