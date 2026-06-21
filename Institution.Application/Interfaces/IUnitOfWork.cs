using Institution.Application.Interfaces.Repositories;

namespace Institution.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        IInstitutionRepository InstitutionRepository { get; }
        IInstitutionUpdateHistoryRepository InstitutionUpdateHistoryRepository { get; }
        IEventRepository EventRepository { get; }
        IDisciplineRepository DisciplineRepository { get; }
        IDisciplineUpdateHistoryRepository DisciplineUpdateHistoryRepository { get; }
        IClassroomRepository ClassroomRepository { get; }
        IStudentClassroomRegistrationRepository StudentClassroomRegistrationRepository { get; }
        IDayLessonRepository DayLessonRepository { get; }
        IStudentDayLessonRepository StudentDayLessonRepository { get; }
        IDayLessonScheduleConfigRepository DayLessonScheduleConfigRepository { get; }
        IDayLessonScheduleConfigHistoryRepository DayLessonScheduleConfigHistoryRepository { get; }

        IAcademicProgramRepository AcademicProgramRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        IRoomRepository RoomRepository { get; }
        IShiftRepository ShiftRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        IProgramEditionRepository ProgramEditionRepository { get; }
        ICurriculumEntryRepository CurriculumEntryRepository { get; }
        IEnrollmentRepository EnrollmentRepository { get; }
        IHolidayRepository HolidayRepository { get; }
        IProgramPeriodRepository ProgramPeriodRepository { get; }
        ICalendarDayRepository CalendarDayRepository { get; }
        IProgressRecordRepository ProgressRecordRepository { get; }
        IClassGroupRepository ClassGroupRepository { get; }
        IClassScheduleRepository ClassScheduleRepository { get; }
        IConflictReportRepository ConflictReportRepository { get; }
        IReportCardLayoutRepository ReportCardLayoutRepository { get; }
    }
}
