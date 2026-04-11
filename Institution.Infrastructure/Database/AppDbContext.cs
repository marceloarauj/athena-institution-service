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
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<ClassroomEntity> Classrooms { get; set; }
        public DbSet<DayLessonEntity> DayLessons { get; set; }
        public DbSet<DisciplineUpdateHistoryEntity> DisciplineUpdateHistories { get; set; }
        public DbSet<StudentClassroomRegistrationEntity> StudentClassroomRegistrations { get; set; }
        public DbSet<StudentClassroomNoteEntity> StudentClassroomNotes { get; set; }
        public DbSet<StudentDayLesson> StudentDayLessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
