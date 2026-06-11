using Core.Entities;  

namespace Core.Repositories.Contract
{
    public interface IGenericRepositories<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAll();
        Task<T?> GetById(int id); 

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);  
    }
}
