using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Data.SeedWorks
{
    public class RepositoryBase<T, Key> : IRepository<T, Key> where T : class
    {
        private readonly DbSet<T> _dbSet;
        protected readonly TPBlogContext _context;
        public RepositoryBase(TPBlogContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);  // Thêm bất đồng bộ
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetByIdAsync(Key id)
        {

            var a = await _dbSet.FindAsync(id);
            return a;
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null && includes.Length > 0)
            {
                query = query.Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(expression);
        }

        //public TaskAdd(T entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
