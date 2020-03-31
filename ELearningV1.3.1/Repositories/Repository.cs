using ELearningV1._3._1.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async void Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async void AddRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await _dbSet.FirstAsync(expression);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T> SingleOrDefault(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.SingleOrDefaultAsync(expression);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
