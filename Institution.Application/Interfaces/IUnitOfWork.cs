using Institution.Application.Interfaces.Repositories;

namespace Institution.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        IInstitutionRepository InstitutionRepository { get; }
        IInstitutionUpdateHistoryRepository InstitutionUpdateHistoryRepository { get; }
        IEventRepository EventRepository { get; }
        IDisciplineRepository DisciplineRepository { get; }
    }
}
