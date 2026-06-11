using Core;
using Core.Entities;
using Core.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Data;
using Stripe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;
        private Hashtable _Repositories;
        private IDbContextTransaction _transaction;

        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
            _Repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        } 
        public async ValueTask DisposeAsync()
        {
            await _dbcontext.DisposeAsync();
        }
        public IGenericRepositories<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;// اسم الجدول
            if (!_Repositories.ContainsKey(key))
            {
                var repository = new GenericRepositories<TEntity>(_dbcontext);
                _Repositories.Add(key, repository);
            }
            return _Repositories[key] as IGenericRepositories<TEntity>;
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbcontext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }
    }
}
