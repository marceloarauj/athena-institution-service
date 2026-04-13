using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassroomController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateClassroom([FromBody] CreateClassroomDto dto)
        {
            var response = await mediator.Send(new CreateClassroomCommand(dto));
            return response.AsResult();
        }

        [HttpGet]
        public async Task<IActionResult> ListClassrooms([FromQuery] ListClassroomsFilterDto filter)
        {
            var response = await mediator.Send(new ListClassroomsCommand(filter));
            return response.AsResult();
        }

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto dto)
        {
            var response = await mediator.Send(new RegisterStudentCommand(dto));
            return response.AsResult();
        }

        [HttpPatch("inactivate-student")]
        public async Task<IActionResult> InactivateStudent([FromBody] InactivateStudentDto dto)
        {
            var response = await mediator.Send(new InactivateStudentCommand(dto));
            return response.AsResult();
        }
    }
}
