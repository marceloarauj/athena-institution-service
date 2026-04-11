using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Institution.Infrastructure.Database
{
    public class UnitOfWork
    (
        AppDbContext context,
        IInstitutionRepository institutionRepository,
        IInstitutionUpdateHistoryRepository institutionUpdateHistoryRepository,
        IEventRepository eventRepository
    ) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                return;

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();

            if (_transaction == null) return;

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public IInstitutionRepository InstitutionRepository { get; } = institutionRepository;
        public IInstitutionUpdateHistoryRepository InstitutionUpdateHistoryRepository { get; } = institutionUpdateHistoryRepository;
        public IEventRepository EventRepository { get; } = eventRepository;
    }
}
