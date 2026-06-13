using Core.Entities;
using System.Linq.Expressions;

namespace Core.Repositories.Contract
{
    public interface IGenericRepositories<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAll();
        Task<T?> GetById(int id); 
        Task<T?> GetById(string userid);
        Task<IReadOnlyList<T>> GetAllWithFilterAsync(Expression<Func<T, bool>> filter); 
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);  
    }
}
