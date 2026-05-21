using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramEditionController(IMediator mediator) : ControllerBase
    {
        // ── Edition CRUD ──────────────────────────────────────────────────────

        [HttpPost]
        public async Task<IActionResult> CreateProgramEdition([FromBody] CreateProgramEditionDto dto)
        {
            var response = await mediator.Send(new CreateProgramEditionCommand(dto));
            return response.AsResult();
        }

        [HttpGet("{programId:guid}")]
        public async Task<IActionResult> ListProgramEditions(Guid programId)
        {
            var response = await mediator.Send(new ListProgramEditionsCommand(programId));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}")]
        public async Task<IActionResult> GetProgramEdition(Guid id)
        {
            var response = await mediator.Send(new GetProgramEditionCommand(id));
            return response.AsResult();
        }

        // ── Curriculum ────────────────────────────────────────────────────────

        [HttpPut("edition/{id:guid}/curriculum")]
        public async Task<IActionResult> BulkUpsertCurriculum(Guid id, [FromBody] BulkUpsertCurriculumDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new BulkUpsertCurriculumCommand(dto));
            return response.AsResult();
        }

        [HttpPut("edition/{id:guid}/curriculum/entry")]
        public async Task<IActionResult> UpsertCurriculumEntry(Guid id, [FromBody] UpsertCurriculumEntryDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new UpsertCurriculumEntryCommand(dto));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/curriculum")]
        public async Task<IActionResult> ListCurriculum(Guid id, [FromQuery] int? gradeOrYear)
        {
            var response = await mediator.Send(new ListCurriculumCommand(id, gradeOrYear));
            return response.AsResult();
        }

        // ── Enrollment ────────────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/enroll")]
        public async Task<IActionResult> EnrollStudent(Guid id, [FromBody] EnrollStudentDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new EnrollStudentCommand(dto));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/enroll/bulk")]
        public async Task<IActionResult> BulkEnrollStudents(Guid id, [FromBody] BulkEnrollStudentsDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new BulkEnrollStudentsCommand(dto));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/enrollments")]
        public async Task<IActionResult> ListEnrollments(Guid id)
        {
            var response = await mediator.Send(new ListEnrollmentsCommand(id));
            return response.AsResult();
        }

        [HttpPatch("edition/{id:guid}/enrollments/{enrollmentId:guid}")]
        public async Task<IActionResult> UpdateEnrollmentStatus(Guid id, Guid enrollmentId, [FromBody] UpdateEnrollmentStatusDto dto)
        {
            var response = await mediator.Send(new UpdateEnrollmentStatusCommand(enrollmentId, dto));
            return response.AsResult();
        }

        // ── Periods ───────────────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/generate-periods")]
        public async Task<IActionResult> GeneratePeriods(Guid id)
        {
            var response = await mediator.Send(new GeneratePeriodsCommand(new GeneratePeriodsDto { ProgramEditionId = id }));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/periods")]
        public async Task<IActionResult> CreatePeriod(Guid id, [FromBody] CreatePeriodDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new CreatePeriodCommand(dto));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/periods")]
        public async Task<IActionResult> ListPeriods(Guid id)
        {
            var response = await mediator.Send(new ListPeriodsCommand(id));
            return response.AsResult();
        }

        // ── Calendar ──────────────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/generate-calendar")]
        public async Task<IActionResult> GenerateCalendar(Guid id)
        {
            var response = await mediator.Send(new GenerateCalendarCommand(new GenerateCalendarDto { ProgramEditionId = id }));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/calendar")]
        public async Task<IActionResult> GetCalendar(Guid id)
        {
            var response = await mediator.Send(new GetCalendarCommand(id));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/calendar/summary")]
        public async Task<IActionResult> GetCalendarSummary(Guid id)
        {
            var response = await mediator.Send(new GetCalendarSummaryCommand(id));
            return response.AsResult();
        }

        // ── Progress ──────────────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/progress")]
        public async Task<IActionResult> BulkRecordProgress(Guid id, [FromBody] BulkRecordProgressDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new BulkRecordProgressCommand(dto));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/progress/record")]
        public async Task<IActionResult> RecordProgress([FromBody] RecordProgressDto dto)
        {
            var response = await mediator.Send(new RecordProgressCommand(dto));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/progress")]
        public async Task<IActionResult> ListProgress(Guid id)
        {
            var response = await mediator.Send(new ListProgressCommand(id));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/promotion/preview")]
        public async Task<IActionResult> GetPromotionPreview(Guid id, [FromQuery] Guid targetEditionId)
        {
            var response = await mediator.Send(new GetPromotionPreviewCommand(new RunPromotionDto { SourceEditionId = id, TargetEditionId = targetEditionId }));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/promotion/run")]
        public async Task<IActionResult> RunPromotion(Guid id, [FromBody] RunPromotionDto dto)
        {
            dto.SourceEditionId = id;
            var response = await mediator.Send(new RunPromotionCommand(dto));
            return response.AsResult();
        }

        // ── Class Groups ──────────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/class-groups")]
        public async Task<IActionResult> CreateClassGroup(Guid id, [FromBody] CreateClassGroupDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new CreateClassGroupCommand(dto));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/class-groups/generate")]
        public async Task<IActionResult> GenerateClassGroups(Guid id, [FromBody] GenerateClassGroupsDto dto)
        {
            dto.ProgramEditionId = id;
            var response = await mediator.Send(new GenerateClassGroupsCommand(dto));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/class-groups/assign-students")]
        public async Task<IActionResult> AssignStudentsToGroups(Guid id)
        {
            var response = await mediator.Send(new AssignStudentsToGroupsCommand(new AssignStudentsToGroupsDto { ProgramEditionId = id }));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/class-groups")]
        public async Task<IActionResult> ListClassGroups(Guid id)
        {
            var response = await mediator.Send(new ListClassGroupsCommand(id));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/class-groups/{groupId:guid}")]
        public async Task<IActionResult> GetClassGroup(Guid id, Guid groupId)
        {
            var response = await mediator.Send(new GetClassGroupCommand(groupId));
            return response.AsResult();
        }

        // ── Schedule ──────────────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/generate-schedule")]
        public async Task<IActionResult> GenerateSchedule(Guid id)
        {
            var response = await mediator.Send(new GenerateScheduleCommand(new GenerateScheduleDto { ProgramEditionId = id }));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/class-groups/{groupId:guid}/schedule")]
        public async Task<IActionResult> GetClassGroupSchedule(Guid id, Guid groupId)
        {
            var response = await mediator.Send(new GetClassGroupScheduleCommand(groupId));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/teachers/{teacherId:guid}/schedule")]
        public async Task<IActionResult> GetTeacherSchedule(Guid id, Guid teacherId)
        {
            var response = await mediator.Send(new GetTeacherScheduleCommand(teacherId, id));
            return response.AsResult();
        }

        // ── Conflict Detection ────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/conflict-detection")]
        public async Task<IActionResult> RunConflictDetection(Guid id)
        {
            var response = await mediator.Send(new RunConflictDetectionCommand(id));
            return response.AsResult();
        }

        [HttpGet("edition/{id:guid}/conflict-report")]
        public async Task<IActionResult> GetConflictReport(Guid id)
        {
            var response = await mediator.Send(new GetConflictReportCommand(id));
            return response.AsResult();
        }

        [HttpPut("edition/{id:guid}/schedule/{scheduleId:guid}")]
        public async Task<IActionResult> OverrideScheduleSlot(Guid id, Guid scheduleId, [FromBody] OverrideScheduleSlotDto dto)
        {
            var response = await mediator.Send(new OverrideScheduleSlotCommand(scheduleId, dto));
            return response.AsResult();
        }

        // ── Publish ───────────────────────────────────────────────────────────

        [HttpPost("edition/{id:guid}/publish-checklist")]
        public async Task<IActionResult> GetPublishChecklist(Guid id)
        {
            var response = await mediator.Send(new GetPublishChecklistCommand(id));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/publish")]
        public async Task<IActionResult> PublishProgramEdition(Guid id)
        {
            var response = await mediator.Send(new PublishProgramEditionCommand(id));
            return response.AsResult();
        }

        [HttpPost("edition/{id:guid}/close")]
        public async Task<IActionResult> CloseProgramEdition(Guid id)
        {
            var response = await mediator.Send(new CloseProgramEditionCommand(id));
            return response.AsResult();
        }
    }
}
