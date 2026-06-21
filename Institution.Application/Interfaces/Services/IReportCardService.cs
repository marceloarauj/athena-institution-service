using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Services
{
    public interface IReportCardService
    {
        Task<byte[]> GeneratePdfAsync(
            InstitutionEntity institution,
            Guid studentId,
            string layoutJson,
            CancellationToken cancellationToken = default);
    }
}
