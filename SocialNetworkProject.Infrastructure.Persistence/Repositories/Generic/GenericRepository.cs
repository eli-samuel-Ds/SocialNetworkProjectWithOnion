using Microsoft.EntityFrameworkCore;
using SocialNetworkProject.Core.Domain.Interfaces.Generic;
using SocialNetworkProject.Infrastructure.Persistence.Contexts;

namespace SocialNetworkProject.Infrastructure.Persistence.Repositories.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly SocialNetworkProjectContext _context;

        public GenericRepository(SocialNetworkProjectContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity?> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAllQuery()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity?> UpdateAsync(int id, TEntity entity)
        {
            var existing = await _context.Set<TEntity>().FindAsync(id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }
    }
}
