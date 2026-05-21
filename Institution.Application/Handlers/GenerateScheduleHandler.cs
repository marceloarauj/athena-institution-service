using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GenerateScheduleHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GenerateScheduleCommand, AthenaApiResponse<ScheduleGenerationResultDto>>
    {
        public async Task<AthenaApiResponse<ScheduleGenerationResultDto>> Handle(GenerateScheduleCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ScheduleGenerationResultDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<ScheduleGenerationResultDto>.NotFound("Program edition not found.");

            if (edition.Status == EditionStatus.Published || edition.Status == EditionStatus.Closed)
                return AthenaApiResponse<ScheduleGenerationResultDto>.UnprocessableEntity("Cannot generate schedule for a published or closed edition.");

            var groups = await unitOfWork.ClassGroupRepository.GetByEditionAsync(edition.Id);
            if (groups.Count == 0)
                return AthenaApiResponse<ScheduleGenerationResultDto>.BadRequest("No class groups found for this edition.");

            var curriculum = await unitOfWork.CurriculumEntryRepository.GetByEditionAsync(edition.Id);
            var allTeachers = await unitOfWork.TeacherRepository.GetByInstitutionAsync(institution.Id);

            // Load full teacher data (subjects + availabilities)
            var teachersWithData = new List<TeacherEntity>();
            foreach (var t in allTeachers)
            {
                var full = await unitOfWork.TeacherRepository.FindByIdAsync(t.Id);
                if (full != null) teachersWithData.Add(full);
            }

            // In-memory conflict tracking
            var teacherBusy = new Dictionary<Guid, HashSet<(DayOfWeek, Guid)>>();
            var roomBusy = new Dictionary<Guid, HashSet<(DayOfWeek, Guid)>>();

            foreach (var t in teachersWithData)
                teacherBusy[t.Id] = [];

            foreach (var g in groups.Where(g => g.RoomId.HasValue))
                roomBusy[g.RoomId!.Value] = [];

            var schedules = new List<ClassScheduleEntity>();
            int totalAssigned = 0;
            int totalUnresolved = 0;

            var workDays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };

            await unitOfWork.ClassScheduleRepository.DeleteByEditionAsync(edition.Id);

            foreach (var group in groups.OrderBy(g => g.GradeOrYear))
            {
                var groupCurriculum = curriculum.Where(c =>
                    c.GradeOrYear == null || c.GradeOrYear == group.GradeOrYear).ToList();

                var slots = group.Shift?.Slots?.OrderBy(s => s.Order).ToList() ?? [];
                if (slots.Count == 0) continue;

                foreach (var entry in groupCurriculum.Where(e => e.WeeklyHours.HasValue && e.WeeklyHours > 0))
                {
                    int hoursToAssign = entry.WeeklyHours!.Value;

                    for (int h = 0; h < hoursToAssign; h++)
                    {
                        bool assigned = false;

                        var eligibleTeachers = teachersWithData.Where(t =>
                            t.Subjects.Any(s => s.SubjectId == entry.SubjectId) &&
                            t.Availabilities.Any(a => group.ShiftId.HasValue && a.ShiftId == group.ShiftId.Value)).ToList();

                        foreach (var day in workDays)
                        {
                            if (assigned) break;

                            foreach (var slot in slots)
                            {
                                if (assigned) break;

                                foreach (var teacher in eligibleTeachers)
                                {
                                    if (!teacher.Availabilities.Any(a => a.DayOfWeek == day && a.ShiftId == group.ShiftId))
                                        continue;

                                    var teacherKey = (day, slot.Id);
                                    if (teacherBusy[teacher.Id].Contains(teacherKey))
                                        continue;

                                    if (group.RoomId.HasValue && roomBusy.TryGetValue(group.RoomId.Value, out var roomSet) && roomSet.Contains(teacherKey))
                                        continue;

                                    var schedule = new ClassScheduleEntity
                                    {
                                        ClassGroupId = group.Id,
                                        ClassGroup = group,
                                        SubjectId = entry.SubjectId,
                                        TeacherId = teacher.Id,
                                        Teacher = teacher,
                                        DayOfWeek = day,
                                        ScheduleSlotId = slot.Id,
                                        ScheduleSlot = slot
                                    };

                                    schedules.Add(schedule);
                                    teacherBusy[teacher.Id].Add(teacherKey);
                                    if (group.RoomId.HasValue)
                                    {
                                        if (!roomBusy.ContainsKey(group.RoomId.Value))
                                            roomBusy[group.RoomId.Value] = [];
                                        roomBusy[group.RoomId.Value].Add(teacherKey);
                                    }

                                    totalAssigned++;
                                    assigned = true;
                                    break;
                                }
                            }
                        }

                        if (!assigned)
                            totalUnresolved++;
                    }
                }
            }

            if (schedules.Count > 0)
                await unitOfWork.ClassScheduleRepository.AddRangeAsync(schedules);

            var status = totalUnresolved == 0 ? GenerationStatus.Success
                : totalAssigned == 0 ? GenerationStatus.Failed
                : GenerationStatus.PartialSuccess;

            var log = new ScheduleGenerationLogEntity
            {
                ProgramEditionId = edition.Id,
                ProgramEdition = edition,
                GeneratedAt = DateTime.UtcNow,
                Status = status,
                TotalAssigned = totalAssigned,
                TotalUnresolved = totalUnresolved
            };

            await unitOfWork.ClassScheduleRepository.AddLogAsync(log);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ScheduleGenerationResultDto>.Ok(new ScheduleGenerationResultDto
            {
                TotalAssigned = totalAssigned,
                TotalUnresolved = totalUnresolved,
                Status = status,
                GeneratedAt = log.GeneratedAt
            });
        }
    }
}
