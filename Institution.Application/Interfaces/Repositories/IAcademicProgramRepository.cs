using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IAcademicProgramRepository
    {
        Task AddAsync(AcademicProgramEntity entity);
        Task<AcademicProgramEntity?> FindByIdAsync(Guid id);
        Task<List<AcademicProgramEntity>> GetByInstitutionAsync(Guid institutionId);
    }
}
