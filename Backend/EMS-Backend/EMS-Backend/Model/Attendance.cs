

using EMS_Backend.Entity;

namespace EMS_Backend.Model
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
      
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public enum AttendanceType
    {
        Present = 1,
        Absent = 2
    }
}
