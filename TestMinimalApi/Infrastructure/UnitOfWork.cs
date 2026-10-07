using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TestMinimalApi.Application;
using TestMinimalApi.Infrastructure.Data;

namespace TestMinimalApi.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public async Task BeginTransactionAsync()
        {
            if (_db.Database.IsRelational())
            {
                _transaction = await _db.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }
    }

}
