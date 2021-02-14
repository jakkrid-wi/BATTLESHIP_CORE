using BATTLESHIP_CORE_EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DBContext _db;
        public Repository(DBContext context)
        {
            _db = context;
        }
        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _db.Set<T>().AddRange(entities);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _db.Set<T>().Where(expression);
        }
        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _db.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _db.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _db.Set<T>().RemoveRange(entities);
        }
    }
}
