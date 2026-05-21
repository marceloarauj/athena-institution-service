using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface ISubjectRepository
    {
        Task AddAsync(SubjectEntity entity);
        Task<SubjectEntity?> FindByIdAsync(Guid id);
        Task<List<SubjectEntity>> GetByProgramAsync(Guid academicProgramId);
    }
}
