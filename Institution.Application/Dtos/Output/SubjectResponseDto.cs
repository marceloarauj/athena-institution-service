using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class SubjectResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public Guid AcademicProgramId { get; set; }
        public DateTime CreatedAt { get; set; }

        public SubjectResponseDto(SubjectEntity e)
        {
            Id = e.Id;
            Name = e.Name;
            Code = e.Code;
            IsActive = e.IsActive;
            AcademicProgramId = e.AcademicProgramId;
            CreatedAt = e.CreatedAt;
        }
    }
}
