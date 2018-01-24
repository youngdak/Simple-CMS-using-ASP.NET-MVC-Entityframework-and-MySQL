using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : EntityBase
    {
        List<T> Get();
        T Get(int? id);
        T Find(int? id);
        T Add(T obj);
        T Update(T obj);
        int Delete(T obj);
        Task<List<T>> GetAsync();
        Task<T> GetAsync(int? id);
        Task<T> FindByKey(params object[] keys);
        Task<T> AddAsync(T obj);
        Task<T> UpdateAsync(T obj);
        Task<int> DeleteAsync(T obj);
        void Dispose();
    }
}