using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ClassroomResponseDto(ClassroomEntity classroom)
    {
        public Guid Id { get; set; } = classroom.Id;
        public int? MaxStudents { get; set; } = classroom.MaxStudents;
        public string Location { get; set; } = classroom.Location;
        public Guid TeacherId { get; set; } = classroom.TeacherId;
        public DateTime StartDate { get; set; } = classroom.StartDate;
        public DateTime EndDate { get; set; } = classroom.EndDate;
        public Guid DisciplineId { get; set; } = classroom.DisciplineId;
        public string DisciplineName { get; set; } = classroom.Discipline.Name;
        public Guid CreatedByUserId { get; set; } = classroom.CreateByUserId;
        public DateTime CreatedAt { get; set; } = classroom.CreatedAt;
    }
}
