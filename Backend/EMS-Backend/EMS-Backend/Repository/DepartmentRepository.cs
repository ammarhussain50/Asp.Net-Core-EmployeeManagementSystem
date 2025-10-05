using EMS_Backend.Data;
using EMS_Backend.Helpers;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly AppDbContext context;

        public DepartmentRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<PagedResult<Department>> GetAllAsync(SearchOptions options)
        {
            var query = context.Departments.AsQueryable();



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

            return new PagedResult<Department>
            {
                TotalCount = totalCount,
                Data = data
            };
        }
    }
}
