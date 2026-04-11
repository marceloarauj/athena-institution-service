using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateInstitution([FromForm] CreateInstitutionDto dto)
        {
            var response = await mediator.Send(new CreateInstitutionCommand(dto));
            return response.AsResult();
        }

        [HttpPut("{alias}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateInstitution(string alias, [FromForm] UpdateInstitutionDto dto)
        {
            var response = await mediator.Send(new UpdateInstitutionCommand(alias, dto));
            return response.AsResult();
        }

        [HttpPut("{alias}/evaluation-variable")]
        public async Task<IActionResult> UpdateEvaluationVariable(string alias, [FromBody] UpdateEvaluationVariableDto dto)
        {
            var response = await mediator.Send(new UpdateEvaluationVariableCommand(dto, alias));
            return response.AsResult();
        }
    }
}
