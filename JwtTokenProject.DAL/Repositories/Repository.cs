using JwtTokenProject.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JwtTokenProject.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly ProjectContext _projectContext;
        private readonly DbSet<T> _dbSet;

        public Repository(ProjectContext projectContext)
        {
            _projectContext = projectContext;
            _dbSet = _projectContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _projectContext.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking().AsQueryable();
        }
    }
}
