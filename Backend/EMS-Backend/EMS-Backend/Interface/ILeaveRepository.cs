using EMS_Backend.Helpers;
using EMS_Backend.Model;

namespace EMS_Backend.Interface
{
    public interface ILeaveRepository : IRepository<Leave>
    {
        //  Admin or general paged list
        Task<PagedResult<Leave>> GetAllAsync(SearchOptions options);

        //  Employee-specific paged list
        Task<PagedResult<Leave>> GetByEmployeeIdAsync(int employeeId, SearchOptions options);
    }
}
