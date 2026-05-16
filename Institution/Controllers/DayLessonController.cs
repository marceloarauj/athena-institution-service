using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/day-lesson")]
    public class DayLessonController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{classroomId}")]
        public async Task<IActionResult> ListDayLessons(Guid classroomId)
        {
            var response = await mediator.Send(new ListDayLessonsCommand(classroomId));
            return response.AsResult();
        }

        [HttpPost("{dayLessonId}/attendance")]
        public async Task<IActionResult> UpdateAttendance(Guid dayLessonId, [FromBody] UpdateAttendanceDto dto)
        {
            var response = await mediator.Send(new UpdateAttendanceCommand(dayLessonId, dto));
            return response.AsResult();
        }

        [HttpPatch("{dayLessonId}/attendance/{studentId}")]
        public async Task<IActionResult> PatchStudentAttendance(Guid dayLessonId, Guid studentId, [FromBody] PatchAttendanceDto dto)
        {
            var response = await mediator.Send(new PatchStudentAttendanceCommand(dayLessonId, studentId, dto));
            return response.AsResult();
        }
    }
}
