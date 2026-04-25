using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Input
{
    public class CreateClassroomDto
    {
        public required Guid DisciplineId { get; set; }
        public required Guid TeacherId { get; set; }
        public required string Location { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public int? MaxStudents { get; set; }
        public bool GenerateDayLessons { get; set; } = false;

        public ClassroomEntity ToEntity(DisciplineEntity discipline, Guid createdByUserId)
        {
            return new ClassroomEntity
            {
                DisciplineId = discipline.Id,
                Discipline = discipline,
                TeacherId = TeacherId,
                Location = Location,
                StartDate = StartDate,
                EndDate = EndDate,
                MaxStudents = MaxStudents,
                CreateByUserId = createdByUserId
            };
        }
    }
}
