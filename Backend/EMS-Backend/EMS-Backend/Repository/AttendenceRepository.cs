using EMS_Backend.Data;
using EMS_Backend.Helpers;
using EMS_Backend.Interface;
using EMS_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend.Repository
{
    public class AttendenceRepository : Repository<Attendance>, IAttendenceRepository
    {
        private readonly AppDbContext context;

        public AttendenceRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<PagedResult<Attendance>> GetAttendanceHistoryAsync(SearchOptions options)
        {
            var query = context.Attendances.AsNoTracking().AsQueryable();

            // Filter by employee
            if (options.EmployeeId.HasValue)
                query = query.Where(x => x.EmployeeId == options.EmployeeId.Value);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination if given
            if (options.PageIndex.HasValue)
            {
                query = query
                    .Skip(options.PageIndex.Value * options.PageSize)
                    .Take(options.PageSize);
            }

            var data = await query.ToListAsync();

            return new PagedResult<Attendance>
            {
                TotalCount = totalCount,
                Data = data
            };
        }
    }
}
