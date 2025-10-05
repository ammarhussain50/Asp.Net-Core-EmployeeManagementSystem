using EMS_Backend.Helpers;
using EMS_Backend.Model;

namespace EMS_Backend.Interface
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        // Repository-level method jo search + paging handle karega
        Task<PagedResult<Department>> GetAllAsync(SearchOptions options);

    }
}
