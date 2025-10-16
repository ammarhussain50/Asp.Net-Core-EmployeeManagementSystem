using EMS_Backend.Helpers;
using EMS_Backend.Model;

namespace EMS_Backend.Interface
{
    public interface IAttendenceRepository : IRepository<Attendance>
    {
        Task<PagedResult<Attendance>> GetAttendanceHistoryAsync(SearchOptions options);
    }
}
