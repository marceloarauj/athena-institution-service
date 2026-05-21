using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ClassScheduleResponseDto
    {
        public Guid Id { get; set; }
        public Guid ClassGroupId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public Guid ScheduleSlotId { get; set; }
        public Guid? ProgramPeriodId { get; set; }

        public ClassScheduleResponseDto(ClassScheduleEntity e)
        {
            Id = e.Id;
            ClassGroupId = e.ClassGroupId;
            SubjectId = e.SubjectId;
            TeacherId = e.TeacherId;
            DayOfWeek = e.DayOfWeek;
            ScheduleSlotId = e.ScheduleSlotId;
            ProgramPeriodId = e.ProgramPeriodId;
        }
    }
}
