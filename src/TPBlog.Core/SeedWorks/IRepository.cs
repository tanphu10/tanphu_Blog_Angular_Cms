using System.Linq.Expressions;

namespace TPBlog.Data.SeedWorks
{
    public interface IRepository<T, Key> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Key id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
