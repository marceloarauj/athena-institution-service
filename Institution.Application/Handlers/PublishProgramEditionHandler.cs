using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class PublishProgramEditionHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<PublishProgramEditionCommand, AthenaApiResponse<ProgramEditionResponseDto>>
    {
        public async Task<AthenaApiResponse<ProgramEditionResponseDto>> Handle(PublishProgramEditionCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Program edition not found.");

            if (edition.Status == EditionStatus.Published)
                return AthenaApiResponse<ProgramEditionResponseDto>.UnprocessableEntity("Edition is already published.");

            if (edition.Status == EditionStatus.Closed)
                return AthenaApiResponse<ProgramEditionResponseDto>.UnprocessableEntity("Cannot publish a closed edition.");

            var checklist = await BuildChecklist(unitOfWork, edition);
            if (!checklist.CanPublish)
            {
                var failedChecks = string.Join(", ", checklist.Checks.Where(c => !c.Passed).Select(c => c.Name));
                return AthenaApiResponse<ProgramEditionResponseDto>.UnprocessableEntity(
                    $"Edition cannot be published. Failed checks: {failedChecks}");
            }

            edition.Status = EditionStatus.Published;
            edition.PublishedAt = DateTime.UtcNow;

            await unitOfWork.ProgramEditionRepository.UpdateStatusAsync(edition);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ProgramEditionResponseDto>.Ok(new ProgramEditionResponseDto(edition));
        }

        internal static async Task<PublishChecklistDto> BuildChecklist(IUnitOfWork unitOfWork, ProgramEditionEntity edition)
        {
            var program = edition.AcademicProgram;
            var checks = new List<PublishCheckItemDto>();

            // 1. Periods generated
            var periods = await unitOfWork.ProgramPeriodRepository.GetByEditionAsync(edition.Id);
            checks.Add(new PublishCheckItemDto
            {
                Name = "PeriodsGenerated",
                Passed = periods.Count > 0,
                Detail = periods.Count > 0 ? $"{periods.Count} period(s)" : "No periods found"
            });

            // 2. Enrollments exist
            var enrollments = await unitOfWork.EnrollmentRepository.GetByEditionAsync(edition.Id);
            checks.Add(new PublishCheckItemDto
            {
                Name = "EnrollmentsExist",
                Passed = enrollments.Count > 0,
                Detail = enrollments.Count > 0 ? $"{enrollments.Count} student(s) enrolled" : "No enrollments found"
            });

            // 3. Calendar + MinSchoolDays (School and PostGraduate)
            if (program.Type != ProgramType.Course)
            {
                int schoolDays = await unitOfWork.CalendarDayRepository.CountSchoolDaysAsync(edition.Id);
                bool calendarOk = schoolDays > 0;
                checks.Add(new PublishCheckItemDto
                {
                    Name = "CalendarGenerated",
                    Passed = calendarOk,
                    Detail = calendarOk ? $"{schoolDays} school day(s)" : "Calendar not generated"
                });

                if (program.MinSchoolDays.HasValue)
                {
                    bool minOk = schoolDays >= program.MinSchoolDays.Value;
                    checks.Add(new PublishCheckItemDto
                    {
                        Name = "MinSchoolDays",
                        Passed = minOk,
                        Detail = $"{schoolDays} >= {program.MinSchoolDays.Value}"
                    });
                }
            }

            // 4. Class groups (School and PostGraduate)
            if (program.Type != ProgramType.Course)
            {
                var groups = await unitOfWork.ClassGroupRepository.GetByEditionAsync(edition.Id);
                checks.Add(new PublishCheckItemDto
                {
                    Name = "ClassGroupsCreated",
                    Passed = groups.Count > 0,
                    Detail = groups.Count > 0 ? $"{groups.Count} group(s)" : "No class groups found"
                });
            }

            // 5. Schedule + ConflictReport (when HasWeeklySchedule)
            if (program.HasWeeklySchedule)
            {
                var schedules = await unitOfWork.ClassScheduleRepository.GetByEditionAsync(edition.Id);
                checks.Add(new PublishCheckItemDto
                {
                    Name = "ScheduleGenerated",
                    Passed = schedules.Count > 0,
                    Detail = schedules.Count > 0 ? $"{schedules.Count} slot(s) assigned" : "Schedule not generated"
                });

                var report = await unitOfWork.ConflictReportRepository.GetLatestByEditionAsync(edition.Id);
                bool noCritical = report != null && report.TotalCritical == 0;
                checks.Add(new PublishCheckItemDto
                {
                    Name = "NoCriticalConflicts",
                    Passed = noCritical,
                    Detail = report == null
                        ? "Conflict detection not run"
                        : noCritical ? "No critical conflicts" : $"{report.TotalCritical} critical conflict(s)"
                });
            }

            bool canPublish = checks.All(c => c.Passed);

            return new PublishChecklistDto
            {
                EditionId = edition.Id,
                CanPublish = canPublish,
                Checks = checks
            };
        }
    }
}
