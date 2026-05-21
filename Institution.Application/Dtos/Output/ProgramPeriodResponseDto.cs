using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ProgramPeriodResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProgramEditionId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int? SchoolDays { get; set; }

        public ProgramPeriodResponseDto(ProgramPeriodEntity e)
        {
            Id = e.Id;
            ProgramEditionId = e.ProgramEditionId;
            Number = e.Number;
            Name = e.Name;
            StartDate = e.StartDate;
            EndDate = e.EndDate;
            SchoolDays = e.SchoolDays;
        }
    }
}
