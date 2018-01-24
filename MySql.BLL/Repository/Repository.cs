using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace MySql.BLL.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly ApplicationDbContext _context;

        protected Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual List<T> Get()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T Get(int? id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Find(int? id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Add(T obj)
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public T Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();

            return obj;
        }

        public int Delete(T obj)
        {
            _context.Set<T>().Attach(obj);
            _context.Set<T>().Remove(obj);
            return _context.SaveChanges();
        }

        public virtual async Task<List<T>> GetAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FindByKey(params object[] keys)
        {
            return await _context.Set<T>().FindAsync(keys);
        }

        public virtual async Task<T> AddAsync(T obj)
        {
            _context.Set<T>().Add(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public virtual async Task<T> UpdateAsync(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return obj;
        }

        public virtual async Task<int> DeleteAsync(T obj)
        {
            _context.Set<T>().Attach(obj);
            _context.Set<T>().Remove(obj);
            return await _context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }
    }
}