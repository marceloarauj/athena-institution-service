using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ClassGroupResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProgramEditionId { get; set; }
        public string Name { get; set; }
        public int? GradeOrYear { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? ShiftId { get; set; }
        public int MaxStudents { get; set; }
        public int StudentCount { get; set; }

        public ClassGroupResponseDto(ClassGroupEntity e)
        {
            Id = e.Id;
            ProgramEditionId = e.ProgramEditionId;
            Name = e.Name;
            GradeOrYear = e.GradeOrYear;
            RoomId = e.RoomId;
            ShiftId = e.ShiftId;
            MaxStudents = e.MaxStudents;
            StudentCount = e.Students?.Count ?? 0;
        }
    }
}
