using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IClassGroupRepository
    {
        Task AddAsync(ClassGroupEntity entity);
        Task AddRangeAsync(List<ClassGroupEntity> entities);
        Task<ClassGroupEntity?> FindByIdAsync(Guid id);
        Task<List<ClassGroupEntity>> GetByEditionAsync(Guid programEditionId);
        Task AddStudentsAsync(List<ClassGroupStudentEntity> students);
        Task<List<ClassGroupStudentEntity>> GetStudentsByGroupAsync(Guid classGroupId);
    }
}
