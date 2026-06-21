using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ReportCardLayoutResponseDto(ReportCardLayoutEntity entity)
    {
        public Guid InstitutionId { get; set; } = entity.InstitutionId;
        public string LayoutJson { get; set; } = entity.LayoutJson;
    }
}
