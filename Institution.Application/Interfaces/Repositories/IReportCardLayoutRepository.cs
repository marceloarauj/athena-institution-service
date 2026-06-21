using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IReportCardLayoutRepository
    {
        Task<ReportCardLayoutEntity?> FindByInstitutionIdAsync(Guid institutionId);
        Task AddAsync(ReportCardLayoutEntity layout);
        void Update(ReportCardLayoutEntity layout);
    }
}
