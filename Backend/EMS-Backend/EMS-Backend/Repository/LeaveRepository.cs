using EMS_Backend.Data;
using EMS_Backend.Helpers;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend.Repository
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private readonly AppDbContext context;

        public LeaveRepository(AppDbContext context): base(context)
        {
            this.context = context;
        }
        public async Task<PagedResult<Leave>> GetAllAsync(SearchOptions options)
        {
            var query = context.Leaves.AsQueryable();
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

            return new PagedResult<Leave>
            {
                TotalCount = totalCount,
                Data = data
            };
        }

        public async Task<PagedResult<Leave>> GetByEmployeeIdAsync(int employeeId, SearchOptions options)
        {
            var query = context.Leaves.Where(x => x.EmployeeId == employeeId);

            var totalCount = await query.CountAsync();

            if (options.PageIndex.HasValue)
            {
                query = query
                    .Skip(options.PageIndex.Value * options.PageSize)
                    .Take(options.PageSize);
            }

            var data = await query.ToListAsync();

            return new PagedResult<Leave>
            {

                TotalCount = totalCount,
                Data = data
            };
        }
    }
}
