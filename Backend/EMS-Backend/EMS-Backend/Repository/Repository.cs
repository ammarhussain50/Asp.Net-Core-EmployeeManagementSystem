using EMS_Backend.Data;
using EMS_Backend.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace EMS_Backend.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext dbContext;
        protected DbSet<T> dbSet;

        public Repository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity!);
        }

        public async Task<List<T>> GetAllAsync()
        {
           return await dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
           return await dbSet.FindAsync(id);
           
        }

        public Task<int> SaveChangesAsync()
        {
           return dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
           dbSet.Update(entity);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
