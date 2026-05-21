using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class RecessResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProgramEditionId { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public RecessResponseDto(RecessEntity e)
        {
            Id = e.Id;
            ProgramEditionId = e.ProgramEditionId;
            Name = e.Name;
            StartDate = e.StartDate;
            EndDate = e.EndDate;
        }
    }
}
