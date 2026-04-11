using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class UpdateDisciplineResponseDto(DisciplineEntity discipline)
    {
        public Guid Id { get; set; } = discipline.Id;
        public string Name { get; set; } = discipline.Name;
        public int StudyHours { get; set; } = discipline.StudyHours;
        public int Credits { get; set; } = discipline.Credits;
        public bool ChargePayment { get; set; } = discipline.ChargePayment;
        public bool Available { get; set; } = discipline.Available;
        public Guid InstitutionId { get; set; } = discipline.InstitutionId;
    }
}
