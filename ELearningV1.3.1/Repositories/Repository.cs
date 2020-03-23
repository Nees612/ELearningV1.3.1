using ELearningV1._3._1.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        protected readonly IdentityDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(IdentityDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> expression)
        {
            return _dbSet.SingleOrDefault(expression);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
