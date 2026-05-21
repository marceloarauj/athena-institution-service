using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class RunConflictDetectionHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<RunConflictDetectionCommand, AthenaApiResponse<ConflictReportResponseDto>>
    {
        public async Task<AthenaApiResponse<ConflictReportResponseDto>> Handle(RunConflictDetectionCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ConflictReportResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<ConflictReportResponseDto>.NotFound("Program edition not found.");

            var schedules = await unitOfWork.ClassScheduleRepository.GetByEditionAsync(edition.Id);
            var curriculum = await unitOfWork.CurriculumEntryRepository.GetByEditionAsync(edition.Id);
            var groups = await unitOfWork.ClassGroupRepository.GetByEditionAsync(edition.Id);

            var items = new List<ConflictItemEntity>();

            // TEACHER_DOUBLE_BOOK
            var teacherSlotGroups = schedules
                .GroupBy(s => (s.TeacherId, s.DayOfWeek, s.ScheduleSlotId))
                .Where(g => g.Count() > 1);

            foreach (var g in teacherSlotGroups)
            {
                items.Add(new ConflictItemEntity
                {
                    Code = "TEACHER_DOUBLE_BOOK",
                    Severity = ConflictSeverity.Critical,
                    Description = $"Teacher {g.Key.TeacherId} is assigned to {g.Count()} classes on {g.Key.DayOfWeek} slot {g.Key.ScheduleSlotId}.",
                    TeacherId = g.Key.TeacherId,
                    DayOfWeek = g.Key.DayOfWeek,
                    ScheduleSlotId = g.Key.ScheduleSlotId
                });
            }

            // ROOM_DOUBLE_BOOK
            var roomSlotGroups = schedules
                .Where(s => s.ClassGroup?.RoomId != null)
                .GroupBy(s => (RoomId: s.ClassGroup!.RoomId!.Value, s.DayOfWeek, s.ScheduleSlotId))
                .Where(g => g.Count() > 1);

            foreach (var g in roomSlotGroups)
            {
                items.Add(new ConflictItemEntity
                {
                    Code = "ROOM_DOUBLE_BOOK",
                    Severity = ConflictSeverity.Critical,
                    Description = $"Room {g.Key.RoomId} has {g.Count()} classes on {g.Key.DayOfWeek} slot {g.Key.ScheduleSlotId}.",
                    DayOfWeek = g.Key.DayOfWeek,
                    ScheduleSlotId = g.Key.ScheduleSlotId
                });
            }

            // MISSING_SUBJECT_HOURS
            foreach (var group in groups)
            {
                var groupSchedules = schedules.Where(s => s.ClassGroupId == group.Id).ToList();
                var groupCurriculum = curriculum.Where(c => c.GradeOrYear == null || c.GradeOrYear == group.GradeOrYear).ToList();

                foreach (var entry in groupCurriculum.Where(e => e.WeeklyHours.HasValue && e.WeeklyHours > 0))
                {
                    int assigned = groupSchedules.Count(s => s.SubjectId == entry.SubjectId);
                    if (assigned < entry.WeeklyHours!.Value)
                    {
                        items.Add(new ConflictItemEntity
                        {
                            Code = "MISSING_SUBJECT_HOURS",
                            Severity = ConflictSeverity.Critical,
                            Description = $"Group '{group.Name}' has {assigned}/{entry.WeeklyHours} weekly hours for subject {entry.SubjectId}.",
                            ClassGroupId = group.Id,
                            SubjectId = entry.SubjectId
                        });
                    }
                }
            }

            // TEACHER_UNAVAILABLE
            foreach (var schedule in schedules)
            {
                var teacher = schedule.Teacher;
                if (teacher == null) continue;

                bool available = teacher.Availabilities.Any(a =>
                    a.DayOfWeek == schedule.DayOfWeek &&
                    schedule.ClassGroup?.ShiftId.HasValue == true &&
                    a.ShiftId == schedule.ClassGroup!.ShiftId!.Value);

                if (!available)
                {
                    items.Add(new ConflictItemEntity
                    {
                        Code = "TEACHER_UNAVAILABLE",
                        Severity = ConflictSeverity.High,
                        Description = $"Teacher {teacher.Name} is not available on {schedule.DayOfWeek} for shift of group {schedule.ClassGroupId}.",
                        TeacherId = teacher.Id,
                        ClassGroupId = schedule.ClassGroupId,
                        DayOfWeek = schedule.DayOfWeek,
                        ScheduleSlotId = schedule.ScheduleSlotId
                    });
                }
            }

            // UNASSIGNED_SUBJECT
            foreach (var group in groups)
            {
                var groupSchedules = schedules.Where(s => s.ClassGroupId == group.Id).ToList();
                var groupCurriculum = curriculum.Where(c => c.GradeOrYear == null || c.GradeOrYear == group.GradeOrYear).ToList();

                foreach (var entry in groupCurriculum.Where(e => e.WeeklyHours.HasValue && e.WeeklyHours > 0))
                {
                    if (!groupSchedules.Any(s => s.SubjectId == entry.SubjectId))
                    {
                        items.Add(new ConflictItemEntity
                        {
                            Code = "UNASSIGNED_SUBJECT",
                            Severity = ConflictSeverity.High,
                            Description = $"Subject {entry.SubjectId} has no schedule assigned in group '{group.Name}'.",
                            ClassGroupId = group.Id,
                            SubjectId = entry.SubjectId
                        });
                    }
                }
            }

            // OVERLOADED_TEACHER
            var teacherLoadGroups = schedules.GroupBy(s => s.TeacherId).Where(g => g.Count() > 30);
            foreach (var g in teacherLoadGroups)
            {
                items.Add(new ConflictItemEntity
                {
                    Code = "OVERLOADED_TEACHER",
                    Severity = ConflictSeverity.Medium,
                    Description = $"Teacher {g.Key} has {g.Count()} weekly slots assigned (max recommended: 30).",
                    TeacherId = g.Key
                });
            }

            await unitOfWork.ConflictReportRepository.DeleteByEditionAsync(edition.Id);

            var report = new ConflictReportEntity
            {
                ProgramEditionId = edition.Id,
                ProgramEdition = edition,
                GeneratedAt = DateTime.UtcNow,
                TotalCritical = items.Count(i => i.Severity == ConflictSeverity.Critical),
                TotalHigh = items.Count(i => i.Severity == ConflictSeverity.High),
                TotalMedium = items.Count(i => i.Severity == ConflictSeverity.Medium),
                Items = items
            };

            foreach (var item in items)
            {
                item.ConflictReport = report;
                item.ConflictReportId = report.Id;
            }

            await unitOfWork.ConflictReportRepository.AddAsync(report);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ConflictReportResponseDto>.Ok(new ConflictReportResponseDto(report));
        }
    }
}
