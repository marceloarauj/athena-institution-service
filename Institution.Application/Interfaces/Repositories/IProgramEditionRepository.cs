using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IProgramEditionRepository
    {
        Task AddAsync(ProgramEditionEntity entity);
        Task<ProgramEditionEntity?> FindByIdAsync(Guid id);
        Task<List<ProgramEditionEntity>> GetByProgramAsync(Guid academicProgramId);
        Task UpdateStatusAsync(ProgramEditionEntity entity);
    }
}
