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
        [HttpPost("{dayLessonId}/attendance")]
        public async Task<IActionResult> UpdateAttendance(Guid dayLessonId, [FromBody] UpdateAttendanceDto dto)
        {
            var response = await mediator.Send(new UpdateAttendanceCommand(dayLessonId, dto));
            return response.AsResult();
        }
    }
}
