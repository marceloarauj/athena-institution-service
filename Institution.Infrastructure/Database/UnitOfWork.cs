using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Institution.Infrastructure.Database
{
    public class UnitOfWork
    (
        AppDbContext context,
        IInstitutionRepository institutionRepository,
        IInstitutionUpdateHistoryRepository institutionUpdateHistoryRepository,
        IEventRepository eventRepository,
        IDisciplineRepository disciplineRepository,
        IDisciplineUpdateHistoryRepository disciplineUpdateHistoryRepository,
        IClassroomRepository classroomRepository,
        IStudentClassroomRegistrationRepository studentClassroomRegistrationRepository,
        IDayLessonRepository dayLessonRepository,
        IStudentDayLessonRepository studentDayLessonRepository,
        IDayLessonScheduleConfigRepository dayLessonScheduleConfigRepository,
        IDayLessonScheduleConfigHistoryRepository dayLessonScheduleConfigHistoryRepository,
        IAcademicProgramRepository academicProgramRepository,
        ISubjectRepository subjectRepository,
        IRoomRepository roomRepository,
        IShiftRepository shiftRepository,
        ITeacherRepository teacherRepository,
        IProgramEditionRepository programEditionRepository,
        ICurriculumEntryRepository curriculumEntryRepository,
        IEnrollmentRepository enrollmentRepository,
        IHolidayRepository holidayRepository,
        IProgramPeriodRepository programPeriodRepository,
        ICalendarDayRepository calendarDayRepository,
        IProgressRecordRepository progressRecordRepository,
        IClassGroupRepository classGroupRepository,
        IClassScheduleRepository classScheduleRepository,
        IConflictReportRepository conflictReportRepository
    ) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                return;

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();

            if (_transaction == null) return;

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public IInstitutionRepository InstitutionRepository { get; } = institutionRepository;
        public IInstitutionUpdateHistoryRepository InstitutionUpdateHistoryRepository { get; } = institutionUpdateHistoryRepository;
        public IEventRepository EventRepository { get; } = eventRepository;
        public IDisciplineRepository DisciplineRepository { get; } = disciplineRepository;
        public IDisciplineUpdateHistoryRepository DisciplineUpdateHistoryRepository { get; } = disciplineUpdateHistoryRepository;
        public IClassroomRepository ClassroomRepository { get; } = classroomRepository;
        public IStudentClassroomRegistrationRepository StudentClassroomRegistrationRepository { get; } = studentClassroomRegistrationRepository;
        public IDayLessonRepository DayLessonRepository { get; } = dayLessonRepository;
        public IStudentDayLessonRepository StudentDayLessonRepository { get; } = studentDayLessonRepository;
        public IDayLessonScheduleConfigRepository DayLessonScheduleConfigRepository { get; } = dayLessonScheduleConfigRepository;
        public IDayLessonScheduleConfigHistoryRepository DayLessonScheduleConfigHistoryRepository { get; } = dayLessonScheduleConfigHistoryRepository;

        public IAcademicProgramRepository AcademicProgramRepository { get; } = academicProgramRepository;
        public ISubjectRepository SubjectRepository { get; } = subjectRepository;
        public IRoomRepository RoomRepository { get; } = roomRepository;
        public IShiftRepository ShiftRepository { get; } = shiftRepository;
        public ITeacherRepository TeacherRepository { get; } = teacherRepository;
        public IProgramEditionRepository ProgramEditionRepository { get; } = programEditionRepository;
        public ICurriculumEntryRepository CurriculumEntryRepository { get; } = curriculumEntryRepository;
        public IEnrollmentRepository EnrollmentRepository { get; } = enrollmentRepository;
        public IHolidayRepository HolidayRepository { get; } = holidayRepository;
        public IProgramPeriodRepository ProgramPeriodRepository { get; } = programPeriodRepository;
        public ICalendarDayRepository CalendarDayRepository { get; } = calendarDayRepository;
        public IProgressRecordRepository ProgressRecordRepository { get; } = progressRecordRepository;
        public IClassGroupRepository ClassGroupRepository { get; } = classGroupRepository;
        public IClassScheduleRepository ClassScheduleRepository { get; } = classScheduleRepository;
        public IConflictReportRepository ConflictReportRepository { get; } = conflictReportRepository;
    }
}
