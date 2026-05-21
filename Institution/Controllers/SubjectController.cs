using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDto dto)
        {
            var response = await mediator.Send(new CreateSubjectCommand(dto));
            return response.AsResult();
        }

        [HttpGet("{programId:guid}")]
        public async Task<IActionResult> ListSubjects(Guid programId)
        {
            var response = await mediator.Send(new ListSubjectsCommand(programId));
            return response.AsResult();
        }
    }
}
