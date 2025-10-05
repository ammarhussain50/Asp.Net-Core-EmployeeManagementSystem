using EMS_Backend.Entity;
using EMS_Backend.Helpers;

namespace EMS_Backend.Interface
{
    public interface IEmployeeRespository : IRepository<Employee>
    {

        // Repository-level method jo search + paging handle karega
        Task  <PagedResult<Employee>> GetAllAsync(SearchOptions options);

    }
}
