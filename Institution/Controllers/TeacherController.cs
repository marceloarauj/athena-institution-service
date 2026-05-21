using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto dto)
        {
            var response = await mediator.Send(new CreateTeacherCommand(dto));
            return response.AsResult();
        }

        [HttpGet]
        public async Task<IActionResult> ListTeachers()
        {
            var response = await mediator.Send(new ListTeachersCommand());
            return response.AsResult();
        }

        [HttpPut("{teacherId:guid}/subjects")]
        public async Task<IActionResult> SetTeacherSubjects(Guid teacherId, [FromBody] SetTeacherSubjectsDto dto)
        {
            dto.TeacherId = teacherId;
            var response = await mediator.Send(new SetTeacherSubjectsCommand(dto));
            return response.AsResult();
        }

        [HttpPut("{teacherId:guid}/availability")]
        public async Task<IActionResult> SetTeacherAvailability(Guid teacherId, [FromBody] SetTeacherAvailabilityDto dto)
        {
            dto.TeacherId = teacherId;
            var response = await mediator.Send(new SetTeacherAvailabilityCommand(dto));
            return response.AsResult();
        }

        [HttpGet("{teacherId:guid}/availability")]
        public async Task<IActionResult> GetTeacherAvailability(Guid teacherId)
        {
            var response = await mediator.Send(new GetTeacherAvailabilityCommand(teacherId));
            return response.AsResult();
        }
    }
}
