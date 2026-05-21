using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class ProgramEditionResponseDto
    {
        public Guid Id { get; set; }
        public Guid AcademicProgramId { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public EditionStatus Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProgramEditionResponseDto(ProgramEditionEntity e)
        {
            Id = e.Id;
            AcademicProgramId = e.AcademicProgramId;
            Name = e.Name;
            StartDate = e.StartDate;
            EndDate = e.EndDate;
            Status = e.Status;
            PublishedAt = e.PublishedAt;
            CreatedAt = e.CreatedAt;
        }
    }
}
