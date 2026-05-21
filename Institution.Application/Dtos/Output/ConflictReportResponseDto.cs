using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class ConflictReportResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProgramEditionId { get; set; }
        public DateTime GeneratedAt { get; set; }
        public int TotalCritical { get; set; }
        public int TotalHigh { get; set; }
        public int TotalMedium { get; set; }
        public List<ConflictItemResponseDto> Items { get; set; } = [];

        public ConflictReportResponseDto(ConflictReportEntity e)
        {
            Id = e.Id;
            ProgramEditionId = e.ProgramEditionId;
            GeneratedAt = e.GeneratedAt;
            TotalCritical = e.TotalCritical;
            TotalHigh = e.TotalHigh;
            TotalMedium = e.TotalMedium;
            Items = e.Items.Select(i => new ConflictItemResponseDto(i)).ToList();
        }
    }

    public class ConflictItemResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public ConflictSeverity Severity { get; set; }
        public string Description { get; set; }

        public ConflictItemResponseDto(ConflictItemEntity e)
        {
            Id = e.Id;
            Code = e.Code;
            Severity = e.Severity;
            Description = e.Description;
        }
    }
}
