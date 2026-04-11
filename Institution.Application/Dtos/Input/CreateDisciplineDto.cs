using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Input
{
    public class CreateDisciplineDto
    {
        public required string Name { get; set; }
        public required int StudyHours { get; set; }
        public required int Credits { get; set; }
        public required bool ChargePayment { get; set; }

        public DisciplineEntity ToEntity(InstitutionEntity institution)
        {
            return new DisciplineEntity
            {
                Name = Name,
                StudyHours = StudyHours,
                Credits = Credits,
                ChargePayment = ChargePayment,
                Available = true,
                CreatedBy = Guid.Empty, // TODO: replace with authenticated user id
                Institution = institution,
                InstitutionId = institution.Id
            };
        }
    }
}
