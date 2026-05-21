using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicProgramController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAcademicProgram([FromBody] CreateAcademicProgramDto dto)
        {
            var response = await mediator.Send(new CreateAcademicProgramCommand(dto));
            return response.AsResult();
        }

        [HttpGet]
        public async Task<IActionResult> ListAcademicPrograms()
        {
            var response = await mediator.Send(new ListAcademicProgramsCommand());
            return response.AsResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAcademicProgram(Guid id)
        {
            var response = await mediator.Send(new GetAcademicProgramCommand(id));
            return response.AsResult();
        }
    }
}
