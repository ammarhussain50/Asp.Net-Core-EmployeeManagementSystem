using EMS_Backend.Data;
using EMS_Backend.Entity;
using EMS_Backend.Helpers;
using EMS_Backend.Interface;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRespository
    {
        private readonly AppDbContext context;

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<PagedResult<Employee>> GetAllAsync(SearchOptions options)
        {
            var query = context.Employees.AsQueryable();

            // Apply search
            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(options.Search) ||
                    x.Phone.Contains(options.Search) ||
                    x.Email.Contains(options.Search));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            if (options.PageIndex.HasValue)
            {
                query = query
                    .Skip(options.PageIndex.Value * options.PageSize)
                    .Take(options.PageSize);
            }

            var data = await query.ToListAsync();

            return new PagedResult<Employee>
            {
                TotalCount = totalCount,
                Data = data
            };
        }
    }
}
