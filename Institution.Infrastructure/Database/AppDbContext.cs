using Institution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<InstitutionEntity> Institutions { get; set; }
        public DbSet<InstitutionUpdateHistoryEntity> InstitutionUpdateHistories { get; set; }
        public DbSet<DisciplineEntity> Disciplines { get; set; }
        public DbSet<DisciplineTopicEntity> DisciplineTopics { get; set; }
        public DbSet<EvaluationVariableEntity> EvaluationVariables { get; set; }
        public DbSet<EvaluationSystemEntity> EvaluationSystems { get; set; }
        public DbSet<ReportCardLayoutEntity> ReportCardLayouts { get; set; }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<ClassroomEntity> Classrooms { get; set; }
        public DbSet<DayLessonEntity> DayLessons { get; set; }
        public DbSet<DisciplineUpdateHistoryEntity> DisciplineUpdateHistories { get; set; }
        public DbSet<StudentClassroomRegistrationEntity> StudentClassroomRegistrations { get; set; }
        public DbSet<StudentClassroomNoteEntity> StudentClassroomNotes { get; set; }
        public DbSet<StudentDayLesson> StudentDayLessons { get; set; }
        public DbSet<DayLessonDisciplineTopic> DayLessonDisciplineTopics { get; set; }
        public DbSet<DayLessonScheduleConfigEntity> DayLessonScheduleConfigs { get; set; }
        public DbSet<DayLessonScheduleConfigHistoryEntity> DayLessonScheduleConfigHistories { get; set; }

        // Academic
        public DbSet<AcademicProgramEntity> AcademicPrograms { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<ProgramEditionEntity> ProgramEditions { get; set; }
        public DbSet<CurriculumEntryEntity> CurriculumEntries { get; set; }
        public DbSet<HolidayEntity> Holidays { get; set; }
        public DbSet<RecessEntity> Recesses { get; set; }
        public DbSet<ProgramPeriodEntity> ProgramPeriods { get; set; }
        public DbSet<CalendarDayEntity> CalendarDays { get; set; }

        // Scheduling
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<ShiftEntity> Shifts { get; set; }
        public DbSet<ScheduleSlotEntity> ScheduleSlots { get; set; }
        public DbSet<TeacherEntity> Teachers { get; set; }
        public DbSet<TeacherSubjectEntity> TeacherSubjects { get; set; }
        public DbSet<TeacherAvailabilityEntity> TeacherAvailabilities { get; set; }
        public DbSet<ClassScheduleEntity> ClassSchedules { get; set; }
        public DbSet<ScheduleGenerationLogEntity> ScheduleGenerationLogs { get; set; }

        // Enrollment
        public DbSet<EnrollmentEntity> Enrollments { get; set; }
        public DbSet<ProgressRecordEntity> ProgressRecords { get; set; }
        public DbSet<ClassGroupEntity> ClassGroups { get; set; }
        public DbSet<ClassGroupStudentEntity> ClassGroupStudents { get; set; }
        public DbSet<ConflictReportEntity> ConflictReports { get; set; }
        public DbSet<ConflictItemEntity> ConflictItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
