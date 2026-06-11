using Core.Entities;
using Core.Repositories.Contract; 
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace Repository
{
    public class GenericRepositories<T> : IGenericRepositories<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;
        public GenericRepositories(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _dbcontext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetById(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbcontext.AddAsync(entity);
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _dbcontext.Update(entity);
            return entity;
        }
        public void Delete(T entity)
        {
            _dbcontext.Remove(entity);
        }
       
    }
}
