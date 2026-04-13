using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IStudentClassroomRegistrationRepository
    {
        Task AddAsync(StudentClassroomRegistrationEntity registration);
        Task<bool> ExistsAsync(Guid studentId, Guid classroomId);
        Task<StudentClassroomRegistrationEntity?> FindByStudentAndClassroomAsync(Guid studentId, Guid classroomId);
    }
}
